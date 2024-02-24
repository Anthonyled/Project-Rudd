using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool lockMovement = false;

    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private int currScale = 0; // -1 == small, 0 == normal, 1 == big
    private bool isFacingRight = true;
    private bool canScale = true;

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
        speed = 5f;
        // Get input from the horizontal axis
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f);

        // Move the player
        transform.Translate(movement * speed * Time.deltaTime);

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.AddForce(new Vector2(movement.x * speed, 0));
        }
    }

    IEnumerator ScaleAnimation(float time, float scale)
    {
        canScale = false;
        Invoke(nameof(UnlockScaling), time);
        float i = 0;
        float rate = 1 / time;

        Vector2 fromScale = transform.localScale;
        Vector2 toScale = fromScale * scale;
        float mass = rb.mass;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            Vector2 p = rb.mass * rb.velocity;
            transform.localScale = Vector2.Lerp(fromScale, toScale, i);
            rb.mass = Mathf.Lerp(mass, mass * scale, i);
            rb.velocity = p / rb.mass;
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
            rb.velocity = new Vector2(rb.velocity.x, 12);
        }

        if (wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpTimer = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }

            Invoke(nameof(StopWallJumping), 0.4f);
        }
    }

    private void OnChangeBigger()
    {
        if (currScale != 1 && canScale)
        {
            StartCoroutine(ScaleAnimation(1, 2f));
            currScale++;
        }
    }

    private void OnChangeSmaller()
    {
        if (currScale != -1 && canScale)
        {
            StartCoroutine(ScaleAnimation(1, 0.5f));
            currScale--;
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

    private void Flip()
    {
        if (isFacingRight && movement.x < 0f || !isFacingRight && movement.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
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