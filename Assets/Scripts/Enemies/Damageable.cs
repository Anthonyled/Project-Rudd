using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    [SerializeField] private float health;

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
