using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Transform groundCheck;

    Vector2 moveVal;
    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float frictionAmount = 0.2f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 70f;
    [Range(0, 1f)] [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float jumpBufferTime = 0.3f;
    [SerializeField] float jumpCoyoteTime = 0.08f;

    [Header("Sizeshifting")]

    [Header("Small")]
    [SerializeField] float smallMass;
    [SerializeField] float smallMoveSpeed;
    [SerializeField] float smallAcceleration;
    [SerializeField] float smallJumpHeight;
    [SerializeField] float smallGravity;

    [Header("Medium")]
    [SerializeField] float mediumMass;
    [SerializeField] float mediumMoveSpeed;
    [SerializeField] float mediumAcceleration;
    [SerializeField] float mediumJumpHeight;
    [SerializeField] float mediumGravity;

    [Header("Big")]
    [SerializeField] float bigMass;
    [SerializeField] float bigMoveSpeed;
    [SerializeField] float bigAcceleration;
    [SerializeField] float bigJumpHeight;
    [SerializeField] float bigGravity;

    private bool isFacingRight;
    
    // Jumping
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isJumping;
    public bool jumpInputReleased;

    private bool controlsActive;

    private bool fallThroughPlatforms;
    private Coroutine fallThroughPlatformsCoroutine;

    private GameObject currentOneWayPlatform;

    // Sizeshifting variables
    [SerializeField] float transformationTime;

    private int curSize = Medium;
    private bool canScale = true;
    enum size
    {
        Small,
        Medium,
        Big
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (transform.localScale.x < 0)
        {
            isFacingRight = false;
        }
        else {isFacingRight = true;}

        controlsActive = true;
    }

    private void FixedUpdate()
    {
        if (controlsActive)
        {
            Run();
            CheckForFlipSprite();
            Friction();
            HandleJump();
            FallThroughPlatforms();
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
    }

    private void HandleJump()
    {
        //resets grounded timer if on ground
        if (IsGrounded())
        {
            lastGroundedTime = jumpCoyoteTime;
        }

        //jumps if jump button pressed within jumpBufferTime seconds and if grounded within jumpCoyoteTime seconds
        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            Jump(jumpHeight);
        }

        //check if jump is complete
        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false;
            jumpInputReleased = false;
        }

        //cut jump early
        if (rb.velocity.y > 0 && isJumping && jumpInputReleased)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
        }
    }

    void Jump(float height)
    {
        float jumpForce = Mathf.Sqrt(height) * Mathf.Sqrt(rb.gravityScale); // Makes all sizes jump to same height
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0f;
        lastJumpTime = 0f;
        isJumping = true;
        jumpInputReleased = false;
    }

    void Run()
    {
        float targetSpeed = moveVal.x * moveSpeed;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
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
        if (IsGrounded() && Mathf.Abs(moveVal.x) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= -Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * amount);
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

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, LayerMask.GetMask("Ground"));
        Debug.Log(grounded ? "grounded" : "not grounded");
        return grounded;
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
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            currentOneWayPlatform = null;
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
            if (curSize != Big && canScale)
            {
                ChangeBigger();
            }
        }
    }

    public void OnChangeSmaller(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (curSize != Small && canScale)
            {
                ChangeSmaller();
            }
        }
    }

    private void ChangeBigger()
    {
        canScale = false;

        size endSize;
        if (curSize == Small) {
            endSize = Medium;
        }
        else if (curSize == Medium) {
            endSize = Big;
        } // curSize shouldn't ever be Big

        StartCoroutine(ScaleAnimation(curSize, endSize));
        curSize = endSize; // May need to change later
        // Possibly increase speed somehow
    }

    private void ChangeSmaller()
    {
        canScale = false;

        size endSize;
        if (curSize == Big) {
            endSize = Medium;
        }
        else if (curSize == Medium) {
            endSize = Small;
        } // curSize shouldn't ever be Small

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
        if (startSize == Small)
        {
            float startMass = smallMass;
            float startMovespeed = smallMoveSpeed;
            float startAcceleration = smallAcceleration;
            float startJumpHeight = smallJumpHeight;
            float startGravity = smallGravity;
        }
        if (startSize == Medium)
        {
            float startMass = mediumMass;
            float startMovespeed = mediumMoveSpeed;
            float startAcceleration = mediumAcceleration;
            float startJumpHeight = mediumJumpHeight;
            float startGravity = mediumGravity;
        }
        if (startSize == Big)
        {
            float startMass = bigMass;
            float startMovespeed = bigMoveSpeed;
            float startAcceleration = bigAcceleration;
            float startJumpHeight = bigJumpHeight;
            float startGravity = bigGravity;
        }

        if (endSize == Big)
        {
            float endMass = bigMass;
            float endMovespeed = bigMoveSpeed;
            float endAcceleration = bigAcceleration;
            float endJumpHeight = bigJumpHeight;
            float endGravity = bigGravity;
        }

        Vector2 fromScale = transform.localScale;
        Vector2 toScale = transform.localScale * scale;

        float startingMass = rb.mass;
        float startingGravityScale = rb.gravityScale;
        while (i < 1)
        {
            i += Time.deltaTime * rate; // i is on a scale from 0 to 1, with 0 being the start of the animation and 1 being the end
            Vector2 p = rb.mass * rb.velocity; // What does this do? -Zach
            
            Vector2 newScale = Vector2.Lerp(fromScale, toScale, i); // Lerp does a linear scale from the start to end
            if (!isFacingRight)
            {
                newScale.x *= -1; // Face left
            }
            transform.localScale = newScale;
            rb.mass = Mathf.Lerp(startingMass, startingMass * scale, i);
            rb.velocity = p / rb.mass; // What does this do? -Zach

            rb.gravityScale = Mathf.Lerp(startingGravityScale, startingGravityScale * scale, i); // Scale gravity
            yield return 0;
        }
    }

    private void UnlockScaling()
    {
        canScale = true;
    }
}
