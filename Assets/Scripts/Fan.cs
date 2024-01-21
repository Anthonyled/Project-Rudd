using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D rb;
    bool applyInitialVelocity = true;
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (rb.position.y > transform.position.y - 0.5 && rb.position.y < transform.position.y + 0.5)
        {
            if (applyInitialVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x + 5, rb.velocity.y / 50);
                applyInitialVelocity = false;
            }
            rb.AddForce(new Vector2(1 / ((rb.position.x - transform.position.x)), 0));
        } else
        {
            applyInitialVelocity = true;
        }
    }
}