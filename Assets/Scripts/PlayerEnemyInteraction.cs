using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemyInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Health h;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        h = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            h.takeDamage();
        }
    }

    public void takeDamage() {
        h.takeDamage();
    }
    // public void die() {
    //     // reset scene
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     // load next level
    //     // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
}
