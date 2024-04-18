using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /*
    public GameObject bullet;
    public Transform bulletPos;
    private float timer;
    */
    [SerializeField] GameObject player;
    private PlayerEnemyInteraction interaction;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        interaction = player.GetComponent<PlayerEnemyInteraction>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer += Time.deltaTime;
        if (timer > 2) {
            timer = 0;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            interaction.takeDamage();
        }
        if (collider.gameObject.CompareTag("Ground")) {
            rb.position = new Vector3(10000,10000,0);
        }
    }
}
