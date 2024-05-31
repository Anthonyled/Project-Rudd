using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    PlayerController p;
    SpriteRenderer renderer;
    Animator animator;
    int damage = 5;
    Collider2D enemyCollision;
    void Start()
    {
        p = player.GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        animator = this.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Meleeeattack");
            Invoke("ActivateHitbox", 0.1f);
            Invoke("DeactivateHitbox", 0.3f);
        }
    }

    void ActivateHitbox()
    {

        if (p.isFacingRight)
        {
            transform.position = new Vector3(player.transform.position.x + 0.8f, player.transform.position.y, 0);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x - 0.8f, player.transform.position.y, 0);
        }

        if (enemyCollision != null)
        {
            Damageable enemy = enemyCollision.GetComponent<Damageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        animator.SetTrigger("onAttack");
    }

    void DeactivateHitbox()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Damageable"))
        {
            enemyCollision = other;
            Debug.Log("enemy collision");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Damageable"))
        {
            enemyCollision = null;
        }
    }
}
