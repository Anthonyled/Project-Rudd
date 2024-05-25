using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player;
    PlayerController p;
    SpriteRenderer renderer;
    void Start()
    {
        p = player.GetComponent<PlayerController>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;   
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
        Vector3 curSize = p.GetDimensions();
        transform.localScale = new Vector3(curSize.x * 1.25f, curSize.x * 1.25f, 0);

        if (p.isFacingRight) {
            transform.position = new Vector3(player.transform.position.x + curSize.x * 1.5f, player.transform.position.y, 0);
        } else {
            transform.position = new Vector3(player.transform.position.x - curSize.x * 1.5f, player.transform.position.y, 0);
        }
        renderer.enabled = true;
    }

    void DeactivateHitbox()
    {
        renderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Debug.Log("enemy collision");
            other.gameObject.SetActive(false);
        }
    }
}
