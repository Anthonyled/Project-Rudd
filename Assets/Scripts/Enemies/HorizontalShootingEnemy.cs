using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalShootingEnemy : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] Vector2 fireDirection;
    [SerializeField] bool facingLeft;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0, 3);
        if (facingLeft)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    private void Shoot()
    {
        Projectile p = (Projectile)Instantiate(projectile, transform.position + (Vector3) fireDirection *2, transform.rotation);
        p.SetSpeed(10);
        p.SetDirection(fireDirection);
    }
}