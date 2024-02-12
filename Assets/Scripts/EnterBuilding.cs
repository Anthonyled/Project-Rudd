using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnterBuilding : MonoBehaviour
{
    private bool inBuilding = false;

    public void Update() {
        
        if (inBuilding && Input.GetButtonDown("EnterBuilding")) {
            SceneManager.LoadScene("Building1");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("collided with building");
        if (other.gameObject.tag == "Player"){
            inBuilding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("exiting building");
        if (other.gameObject.tag == "Player"){
            inBuilding = false;
        }
    }

}

