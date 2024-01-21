using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] int speed;
    Vector2 movement;
    Rigidbody2D rb;
    bool isGrounded = true;
    int currScale = 0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.position += movement * speed * Time.deltaTime;
    }

    void OnMove(InputValue MovementValue)
    {
        Vector2 movementVector = MovementValue.Get<Vector2>().normalized;
        movement = new Vector2(movementVector.x, 0);
    }

    void OnJump()
    {      
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, 250));
        }
    }

    void OnChangeBigger()
    {
        if (currScale != 1)
        {
            transform.localScale *= 2;
            rb.mass *= 2;
            currScale++;
        }
    }

    void OnChangeSmaller()
    {
        if (currScale != -1)
        {
            transform.localScale *= 0.5f;
            rb.mass *= 0.5f;
            currScale--;
        }
    }

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