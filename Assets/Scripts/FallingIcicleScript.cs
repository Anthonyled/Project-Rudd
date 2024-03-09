using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingIcicleScript : MonoBehaviour
{
    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            rb.isKinematic = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            Debug.Log("Hit the player!");
        }
    }
}
