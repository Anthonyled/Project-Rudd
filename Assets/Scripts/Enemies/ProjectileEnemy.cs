using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public Projectile bullet;

    private float timer;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletHeight;
    [SerializeField] float fireDelay;
    Vector3 fireDirection = new Vector3 (0, -1, 0);

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fireDelay) {
            timer = 0;
            Projectile p = (Projectile) Instantiate(bullet, transform.position + (Vector3)fireDirection * 2, transform.rotation);
            p.SetSpeed(10);
            p.SetDirection(fireDirection);
        }
    }

}