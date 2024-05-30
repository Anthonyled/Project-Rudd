using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    PlayerController p;
    SpriteRenderer renderer;
    int damage = 5;
    void Start()
    {
        p = player.GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            Debug.Log("Meleeeattack");
            Invoke("ActivateHitbox", 0.1f);
            Invoke("DeactivateHitbox", 0.3f);
        }
    }

     void ActivateHitbox()
    {
        if (p.isFacingRight) {
            transform.position = new Vector3(player.transform.position.x + 0.8f, player.transform.position.y, 0);
        } else {
            transform.position = new Vector3(player.transform.position.x - 0.8f, player.transform.position.y, 0);
        }
        renderer.enabled = true;
    }

    void DeactivateHitbox()
    {
        //renderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy") && renderer.enabled) {
            Debug.Log("enemy collision");
            Damageable enemy = other.GetComponent<Damageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
