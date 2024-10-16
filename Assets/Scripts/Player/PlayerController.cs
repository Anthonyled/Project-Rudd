using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Vector2 moveVal;
    private float moveSpeed = 10f;
    private float acceleration = 5f;
    private float deceleration = 10f;
    [SerializeField] private float frictionAmount;
    private float jumpHeight = 70f;

    private float oldMoveSpeed;
    private float oldAcceleration;
    private float oldDeceleration;
    private float oldJumpHeight;

    [Header("Jump")]
    [Range(0, 1f)][SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float jumpBufferTime = 0.3f;
    [SerializeField] float jumpCoyoteTime = 0.08f;

    [Header("Sizeshifting")]

    [Range(0, 1f)][SerializeField] float sizeShiftBoostFactor;
    private int inAirShrinkBoostsAvailable; // Number of times shrinking mid air will give a speed boost

    [Header("Small")]
    [SerializeField] float smallMass;
    [SerializeField] Vector2 smallScale;
    [SerializeField] float smallMoveSpeed;
    [SerializeField] float smallAcceleration;
    [SerializeField] float smallDeceleration;
    [SerializeField] float smallJumpHeight;
    [SerializeField] float smallGravity;

    [Header("Medium")]
    [SerializeField] float mediumMass;
    [SerializeField] Vector2 mediumScale;
    [SerializeField] float mediumMoveSpeed;
    [SerializeField] float mediumAcceleration;
    [SerializeField] float mediumDeceleration;
    [SerializeField] float mediumJumpHeight;
    [SerializeField] float mediumGravity;

    [Header("Big")]
    [SerializeField] float bigMass;
    [SerializeField] Vector2 bigScale;
    [SerializeField] float bigMoveSpeed;
    [SerializeField] float bigAcceleration;
    [SerializeField] float bigDeceleration;
    [SerializeField] float bigJumpHeight;
    [SerializeField] float bigGravity;

    public bool isFacingRight;

    // Jumping
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isJumping;
    public bool jumpInputReleased;

    private bool controlsActive;

    private bool fallThroughPlatforms;
    private Coroutine fallThroughPlatformsCoroutine;

    private GameObject currentOneWayPlatform;
    private GameObject currentMovingPlatform;

    private bool onIce;
    [Range(0, 1f)][SerializeField] float iceSlipperiness;

    // Sizeshifting variables
    [SerializeField] float transformationTime;

    private size curSize;
    private bool canScale = true;
    public enum size
    {
        Small,
        Medium,
        Big
    };

    [SerializeField] Vector2 respawnCoords;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpTimer;
    private Vector2 wallJumpingPower = new Vector2(12f, 12f);

    [SerializeField] Projectile projectile;
    private float fireCooldownStart = -3;
    private int ammo = 10000;
    private PlayerEnemyInteraction playerEnemyInteraction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (transform.localScale.x < 0)
        {
            isFacingRight = false;
        }
        else { isFacingRight = true; }

        curSize = size.Medium;
        rb.mass = mediumMass;
        transform.localScale = mediumScale;
        moveSpeed = mediumMoveSpeed;
        acceleration = mediumAcceleration;
        deceleration = mediumDeceleration;
        jumpHeight = mediumJumpHeight;
        rb.gravityScale = mediumGravity;
        oldMoveSpeed = moveSpeed;
        oldAcceleration = acceleration;
        oldDeceleration = deceleration;
        oldJumpHeight = jumpHeight;

        inAirShrinkBoostsAvailable = 2;

        controlsActive = true;

        playerEnemyInteraction = GetComponent<PlayerEnemyInteraction>();
        currentMovingPlatform = null;
    }

    private void FixedUpdate()
    {
        //Debug.Log(inAirShrinkBoostsAvailable);
        if (controlsActive)
        {
            Run();
            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                CheckForFlipSprite();
            }
            Friction();
            HandleJump();
            FallThroughPlatforms();
        }

        if (rb.position.y < -100)
        {
            rb.position = respawnCoords;
            rb.velocity = Vector2.zero;
        }


        if (rb.velocity.x < 1 && rb.velocity.x > -1)
        {
            animator.SetBool("onMove", false);
        }
        else
        {
            animator.SetBool("onMove", true);

        }

        #region timers
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        #endregion
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVal = context.ReadValue<Vector2>(); // Gets the only Vector2 from the Input System
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) // Jump button pressed
        {
            lastJumpTime = jumpBufferTime; // Start buffering a jump
        }
        if (context.canceled) // Jump button released
        {
            jumpInputReleased = true;
        }

        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpTimer = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                FlipSprite();
            }

            Invoke(nameof(StopWallJumping), 0.4f);
        }
    }

    private void HandleJump()
    {
        //resets grounded timer if on ground
        if (IsGrounded())
        {
            inAirShrinkBoostsAvailable = 2;
            lastGroundedTime = jumpCoyoteTime;
            if (jumpInputReleased && lastJumpTime <= 0)
                jumpInputReleased = false;
        }

        //jumps if jump button pressed within jumpBufferTime seconds and if grounded within jumpCoyoteTime seconds
        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping && !jumpInputReleased)
        {
            Jump(jumpHeight);
        }

        // check if jump is complete
        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false;
            jumpInputReleased = false;
        }

        //cut jump early
        if (isJumping && jumpInputReleased)
        {
            jumpInputReleased = false;
            rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
        }
    }

    void Jump(float height)
    {
        float jumpForce = Mathf.Sqrt(height) * Mathf.Sqrt(rb.gravityScale) * rb.mass; // Makes all sizes jump to same height
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0f;
        lastJumpTime = 0f;
        isJumping = true;
        jumpInputReleased = false;
    }

    void Run()
    {
        float baseSpeed = 0f;
        if (currentMovingPlatform != null) {
            Rigidbody2D platformrb = currentMovingPlatform.GetComponent<Rigidbody2D>();
            baseSpeed = platformrb.velocity.x;
        }

        float targetSpeed = baseSpeed + (moveVal.x * moveSpeed);
        float speedDif = targetSpeed - rb.velocity.x;

        float accelRate = acceleration;
        if (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.velocity.x) || Mathf.Abs(targetSpeed) < 0.01f)
        {
            accelRate = deceleration;
        }

        if (onIce)
        {
            accelRate *= iceSlipperiness;
        }
        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right);
    }

    void CheckForFlipSprite()
    {
        if (isFacingRight && moveVal.x < 0f)
        {
            FlipSprite();
        }
        else if (!isFacingRight && moveVal.x > 0f)
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Friction()
    {
        if (IsGrounded() && Mathf.Abs(rb.velocity.x) > moveSpeed)
        {
            float frictionForce = frictionAmount * (Mathf.Abs(rb.velocity.x) - moveSpeed) * -Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * frictionForce);
        }
    }

    public void SetAccelRate(float a)
    {
        acceleration = a;
    }

    public void SetDecelRate(float d)
    {
        deceleration = d;
    }

    public float GetAccelRate()
    {
        return acceleration;
    }

    public float GetDecelRate()
    {
        return deceleration;
    }

    public void SetXVelocity(float v)
    {
        rb.velocity = new Vector2(v, rb.velocity.y);
    }

    public bool IsGrounded()
    {
        float overLapRadius = 0.2f * transform.localScale.y / mediumScale.y; // Scales with current size
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, overLapRadius, groundLayer);
        return grounded;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveVal.x != 0f)
        {
            Debug.Log("Wall Jumping");
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void FallThroughPlatforms()
    {
        if ((moveVal.y < -0.5f && currentOneWayPlatform != null) || fallThroughPlatforms)
        {
            fallThroughPlatformsCoroutine = StartCoroutine(FallThroughPlatformsCoroutine());
        }
        else
        {
            fallThroughPlatformsCoroutine = null;
        }
    }

    IEnumerator FallThroughPlatformsCoroutine()

    {
        if (currentOneWayPlatform != null)
        {
            BoxCollider2D currentPlatformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), currentPlatformCollider, true);
            yield return new WaitForSecondsRealtime(0.25f);
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), currentPlatformCollider, false);
        }
        else
        {
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            currentOneWayPlatform = other.gameObject;
        }

        if (other.gameObject.tag == "Ice")
        {
            onIce = true;
        }

        if (other.gameObject.tag == "Kill")
        {
            playerEnemyInteraction.die();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            currentOneWayPlatform = null;
        }

        if (other.gameObject.tag == "Ice")
        {
            onIce = false;
        }
    }

    public void SetControlsActive(bool flag)
    {
        controlsActive = flag;
    }

    public void SetFallThroughPlatforms(bool flag)
    {
        fallThroughPlatforms = flag;
        if (!flag && fallThroughPlatformsCoroutine != null)
        {
            StopCoroutine(fallThroughPlatformsCoroutine);
        }
    }

    public Vector2 GetMoveVal()
    {
        return moveVal;
    }

    // Sizeshifting Functions

    public void OnChangeBigger(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (curSize != size.Big && canScale)
            {
                ChangeBigger();
            }
        }
    }

    public void OnChangeSmaller(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (curSize != size.Small && canScale)
            {
                ChangeSmaller();
            }
        }
    }

    private void ChangeBigger()
    {
        canScale = false;

        size endSize;
        if (curSize == size.Small)
        {
            endSize = size.Medium;
        }
        else if (curSize == size.Medium)
        {
            endSize = size.Big;
        }
        else
        { // curSize shouldn't ever be Big
            Debug.Log("ERROR: called ChangeBigger when big.");
            endSize = size.Big;
        }

        StartCoroutine(ScaleAnimation(curSize, endSize));
        curSize = endSize; // May need to change later
        // Possibly increase speed somehow
    }

    private void ChangeSmaller()
    {
        canScale = false;

        size endSize;
        if (curSize == size.Big)
        {
            endSize = size.Medium;
        }
        else if (curSize == size.Medium)
        {
            endSize = size.Small;
        }
        else
        { // curSize shouldn't ever be Small
            Debug.Log("ERROR: called ChangeSmaller when small.");
            endSize = size.Small;
        }

        StartCoroutine(ScaleAnimation(curSize, endSize));
        curSize = endSize; // May need to change later
        // Possibly increase speed somehow
    }

    IEnumerator ScaleAnimation(size startSize, size endSize)
    {
        Invoke(nameof(UnlockScaling), transformationTime);
        float i = 0;
        float rate = 1 / transformationTime;

        // Feel free to clean this section up if there's a better way to store these values
        float startMass = rb.mass;
        Vector2 startScale = transform.localScale;
        startScale.x = Mathf.Abs(startScale.x);

        float startMovespeed = moveSpeed;
        float startAcceleration = acceleration;
        float startDeceleration = deceleration;
        float startJumpHeight = jumpHeight;
        float startGravity = rb.gravityScale;

        float endMass = 0;
        Vector2 endScale = Vector2.zero;
        float endMovespeed = 0;
        float endAcceleration = 0;
        float endDeceleration = 0;
        float endJumpHeight = 0;
        float endGravity = 0;

        if (endSize == size.Small)
        {
            endMass = smallMass;
            endScale = smallScale;
            endMovespeed = smallMoveSpeed;
            endAcceleration = smallAcceleration;
            endDeceleration = smallDeceleration;
            endJumpHeight = smallJumpHeight;
            endGravity = smallGravity;
        }

        if (endSize == size.Medium)
        {
            endMass = mediumMass;
            endScale = mediumScale;
            endMovespeed = mediumMoveSpeed;
            endAcceleration = mediumAcceleration;
            endDeceleration = mediumDeceleration;
            endJumpHeight = mediumJumpHeight;
            endGravity = mediumGravity;
        }

        if (endSize == size.Big)
        {
            endMass = bigMass;
            endScale = bigScale;
            endMovespeed = bigMoveSpeed;
            endAcceleration = bigAcceleration;
            endDeceleration = bigDeceleration;
            endJumpHeight = bigJumpHeight;
            endGravity = bigGravity;
        }

        float startVelocity = rb.velocity.x;

        float endVelocity = startVelocity;
        // Give speed boost if shrinking
        if (startScale.y > endScale.y && inAirShrinkBoostsAvailable > 0)
        {
            endVelocity = startVelocity * (1 + sizeShiftBoostFactor);
            inAirShrinkBoostsAvailable--;
        }

        while (i < 1)
        {
            i += Time.deltaTime * rate; // i is on a scale from 0 to 1, with 0 being the start of the animation and 1 being the end

            rb.mass = Mathf.Lerp(startMass, endMass, i); // Lerp does a linear scale from the start to end
            moveSpeed = Mathf.Lerp(startMovespeed, endMovespeed, i);
            acceleration = Mathf.Lerp(startAcceleration, endAcceleration, i);
            deceleration = Mathf.Lerp(startDeceleration, endDeceleration, i);
            jumpHeight = Mathf.Lerp(startJumpHeight, endJumpHeight, i);
            rb.gravityScale = Mathf.Lerp(startGravity, endGravity, i);

            Vector2 newScale = Vector2.Lerp(startScale, endScale, i);

            newScale.x = Mathf.Abs(newScale.x) * (isFacingRight ? 1 : -1);

            transform.localScale = newScale;

            // Give speed boost if shrinking
            rb.velocity = new Vector2(Mathf.Lerp(startVelocity, endVelocity, i), rb.velocity.y);

            yield return 0;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Here");
        if (Time.time - fireCooldownStart > 1 && ammo > 0)
        {
            Projectile p = (Projectile)Instantiate(projectile, transform.position, transform.rotation);
            p.SetSpeed(20);

            Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)(offset - transform.position);
            p.SetDirection(direction.normalized);

            fireCooldownStart = Time.time;
            ammo--;
        }
    }

    private void UnlockScaling()
    {
        canScale = true;
    }

    public size GetSize()
    {
        return curSize;
    }

    public void lockMovement()
    {
        if (moveSpeed != 0)
        {
            oldMoveSpeed = moveSpeed;
            oldAcceleration = acceleration;
            oldDeceleration = deceleration;
            oldJumpHeight = jumpHeight;
            print(moveSpeed + " " + acceleration + " " + deceleration + " " + jumpHeight);
            moveSpeed = 0;
            acceleration = 0;
            deceleration = 0;
            jumpHeight = 0;
            canScale = false;
        }
    }

    public void unlockMovement()
    {
        moveSpeed = oldMoveSpeed;
        acceleration = oldAcceleration;
        jumpHeight = oldJumpHeight;
        canScale = true;
        print(moveSpeed + " " + acceleration + " " + deceleration + " " + jumpHeight);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            LevelManager.levelTracker.Add(SceneManager.GetActiveScene().name, true);
            Debug.Log(LevelManager.levelTracker.Count);
            SceneManager.LoadScene("Overworld");
        }


        if (other.CompareTag("HorizontalPlatform"))
        {
            Debug.Log("Touching horizontal moving platform");
            currentMovingPlatform = other.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wind"))
        {
            Wind w = collision.GetComponent<Wind>();
            rb.AddForce(w.direction * w.force * Time.deltaTime);
            rb.velocity = new Vector2(rb.velocity.x, Math.Clamp(rb.velocity.y, -10, 15));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HorizontalPlatform"))
        {
            currentMovingPlatform = null;
        }
    }
    public void MovingPlatformChange(float speed, HorizontalMovingPlatform other)
    {
        if (other == currentMovingPlatform) {
            SetXVelocity(speed);
        }
    }
}