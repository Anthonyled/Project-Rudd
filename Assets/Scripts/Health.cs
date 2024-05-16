using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    int MAXHEALTH = 3;
    int health;
    // Start is called before the first frame update
    void Start()
    {
        health = MAXHEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage()
    {
        health--;
        if (health == 0) {
            Debug.Log("player died");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public int getHealth() {
        return health;
    }

    public int getMaxHealth() {
        return MAXHEALTH;
    }
}
