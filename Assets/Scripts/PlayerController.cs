using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool lockMovement = false;

    [SerializeField] int speed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private int currScale = 0; // -1 == small, 0 == normal, 1 == big

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (lockMovement)
        {
            movement = Vector2.zero;
        }
        if (rb.velocity.x < speed)
        {
            rb.AddForce(movement);
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
    
    // Input handling
    private void OnMove(InputValue MovementValue)
    {
        if (!lockMovement && (isGrounded || currScale == 0))
        {
            Vector2 movementVector = MovementValue.Get<Vector2>().normalized;
            movement = new Vector2(movementVector.x, 0);
        } 
    }

    private void OnJump()
    {      
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, 150));
        }
    }

    private void OnChangeBigger()
    {
        if (currScale != 1)
        {
            Vector2 p = rb.mass * rb.velocity;
            transform.localScale *= 2;
            rb.mass *= 2;
            currScale++;
            rb.velocity = p / rb.mass;
        }
    }

    private void OnChangeSmaller()
    {
        if (currScale != -1)
        {
            Vector2 p = rb.mass * rb.velocity;
            transform.localScale *= 0.5f;
            rb.mass *= 0.5f;
            currScale--;
            rb.velocity = p / rb.mass;
        }
    }

    // Prevention of double jumps
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}