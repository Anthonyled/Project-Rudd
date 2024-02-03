using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnterBuilding : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("HEY");
        if (other.gameObject.tag == "Player"){
            SceneManager.LoadScene("Building1");
        }
    }
}

