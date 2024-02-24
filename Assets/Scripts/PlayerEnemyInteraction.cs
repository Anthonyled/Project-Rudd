using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemyInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            die();
        }
    }

    public void die() {
        // reset scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // load next level
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
