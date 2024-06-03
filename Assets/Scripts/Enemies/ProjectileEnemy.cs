using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public Projectile bullet;
    public Transform bulletPos;
    private Rigidbody2D bulletRb;
    private Rigidbody2D rb;
    private float timer;
    [SerializeField] GameObject player;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletHeight;
    [SerializeField] float fireDelay;
    Vector3 fireDirection = new Vector3 (0, -1, 0);
    private PlayerEnemyInteraction interaction;
    // Start is called before the first frame update
    void Start()
    {
        interaction = player.GetComponent<PlayerEnemyInteraction>();
        bulletPos = bullet.transform;
        bulletRb = bullet.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
    }

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