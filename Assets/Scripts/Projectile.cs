using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject player;
    int speed = 0;
    [SerializeField] int damage;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector2)((offset - transform.position));
        direction.Normalize();
        rb.velocity = direction * speed;
        initialPosition = rb.position;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
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
}
