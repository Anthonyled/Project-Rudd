using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed;
    private PlayerController controller;

    private Rigidbody2D rb;
    private Transform playerPoint;
    private PlayerEnemyInteraction interaction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = player.GetComponent<PlayerController>();
        playerPoint = player.transform;
        interaction = player.GetComponent<PlayerEnemyInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - playerPoint.position.x > 0) {
            if (!controller.isFacingRight) {
                Vector2 diff = playerPoint.position - transform.position;
                float magnitude = Mathf.Sqrt(diff.x*diff.x + diff.y*diff.y);
                diff /= magnitude;
                rb.velocity = diff * speed;
            }
            else {
                rb.velocity = new Vector2(0, 0);
            }
        }
        else {
            if (controller.isFacingRight) {
                Vector2 diff = playerPoint.position - transform.position;
                float magnitude = Mathf.Sqrt(diff.x*diff.x + diff.y*diff.y);
                diff /= magnitude;
                rb.velocity = diff * speed;
            }
            else {
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Player")) {
            interaction.takeDamage();
        }
    }
}
