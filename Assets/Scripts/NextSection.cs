using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class NextSection : MonoBehaviour
{

    [SerializeField] private UnityEvent enterSectionTwo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enterSectionTwo?.Invoke();
        }
    }
}
