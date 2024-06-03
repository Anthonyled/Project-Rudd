using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    int speed = 0;
    int damage = 10;
    Vector2 direction;

    private Rigidbody2D rb;
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        initialPosition = rb.position;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Damageable") || collision.CompareTag("Player"))
        {
            Damageable enemy = collision.GetComponent<Damageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer))
        {
            Destroy(gameObject);
        }
    }
    public void SetSpeed(int s)
    {
        speed = s;
    }

    public void SetDirection(Vector2 d)
    {
        direction = d;
    }
}
