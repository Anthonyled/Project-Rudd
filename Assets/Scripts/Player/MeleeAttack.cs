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
    List<Collider2D> enemyCollision = new List<Collider2D>();
    void Start()
    {
        p = player.GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        animator = player.GetComponent<Animator>();
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
        animator.SetTrigger("onAttack");
        if (enemyCollision != null)
        {
            foreach (Collider2D coll in enemyCollision) { 
                Damageable enemy = coll.GetComponent<Damageable>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    void DeactivateHitbox()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Damageable"))
        {
            if (!enemyCollision.Contains(other))
            {
                enemyCollision.Add(other);
            }
            Debug.Log("enemy collision");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        // Changing colour according to list 
        if (enemyCollision.Contains(other))
        {
            enemyCollision.Remove(other);
        }
    }
}
