// Something is weird with how gravity scale and speed affect each other. Currently, the big form cannot
// move without jumping. Something to do with friction.

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool lockMovement = false;

    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpHeight;
    private Vector2 movement;
    private Rigidbody2D rb;
    private int currScale = 0; // -1 == small, 0 == normal, 1 == big
    public bool isFacingRight = true;
    private bool canScale = true;
    [SerializeField] Vector2 respawnCoords;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpTimer;
    private Vector2 wallJumpingPower = new Vector2(4f, 12f);

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        if (!isWallJumping) // Face same way during wall jump
        {
            CheckForFlip(); // Flip character if movement doesn't match current direction
        }

        if (rb.position.y < -10)
        {
            rb.position = respawnCoords;
            rb.velocity = Vector2.zero;
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping && !lockMovement)
        {
            rb.AddForce(new Vector2(movement.x * speed, 0));
        }
    }

    IEnumerator ScaleAnimation(float time, float scale)
    {
        Invoke(nameof(UnlockScaling), time);
        float i = 0;
        float rate = 1 / time;
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

    // Input handling
    private void OnMove(InputValue MovementValue)
    {
        Vector2 movementVector = MovementValue.Get<Vector2>().normalized;
        movement = new Vector2(movementVector.x, 0);
    }

    private void OnJump()
    {      
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight) * Mathf.Sqrt(rb.gravityScale)); // Makes all sizes jump to same height
        }

        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpTimer = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), 0.4f);
        }
    }

    private void OnChangeBigger()
    {
        if (currScale != 1 && canScale)
        {
            canScale = false;
            StartCoroutine(ScaleAnimation(1, 2f));
            speed *= 1.5f;
            currScale++;
        }
    }

    private void OnChangeSmaller()
    {
        if (currScale != -1 && canScale)
        {
            canScale = false;
            StartCoroutine(ScaleAnimation(1, 0.5f));
            currScale--;
            speed /= 1.5f;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && movement.x != 0f)
        {
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

    private void CheckForFlip()
    {
        if (movement.x < 0 && transform.localScale.x > 0 || movement.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene("Building1");
        }
    }



    public Vector2 GetPosition()
    {
        return rb.position;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public int GetSize()
    {
        return currScale;
    }

    public float GetMass()
    {
        return rb.mass;
    }

    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
}