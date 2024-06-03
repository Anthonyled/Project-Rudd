using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemyInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            takeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            takeDamage();
        }
    }

    public void takeDamage() {
        health.takeDamage();
    }

    public void die() {
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // load next level
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
