using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float health;

    public Enemy(int health)
    {
        this.health = health;
    }

    private void FixedUpdate()
    {
        Die();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Die()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
