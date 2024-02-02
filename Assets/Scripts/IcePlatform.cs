using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    [SerializeField] int timeToBreak;
    bool breakPlatform = false;
    float startTime;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!breakPlatform)
            {
                Invoke(nameof(BreakPlatform), timeToBreak);
                breakPlatform = true;
            }
        }
    }

    private void BreakPlatform()
    {
        gameObject.SetActive(false);
    }

}
