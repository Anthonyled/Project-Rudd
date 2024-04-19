using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalShootingEnemy : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] Vector2 fireDirection;

    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0, 3);
    }

    private void Shoot()
    {
        Projectile p = (Projectile)Instantiate(projectile, transform.position + (Vector3) fireDirection, transform.rotation);
        p.SetSpeed(10);
        p.SetDirection(fireDirection);
    }
}