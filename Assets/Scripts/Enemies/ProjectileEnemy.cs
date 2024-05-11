using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private Rigidbody2D bulletRb;
    private Rigidbody2D rb;
    private float timer;
    [SerializeField] GameObject player;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletHeight;
    [SerializeField] float fireDelay;
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
            bulletRb.position = transform.position;
            bulletRb.velocity = new Vector2(-bulletSpeed + rb.velocity.x, bulletHeight);
        }
    }

}