using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    [SerializeField] int timeToBreak;
    bool breakPlatform = false;
    float startTime;

    public void Update()
    {
        if (breakPlatform && (Time.time - startTime > timeToBreak))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!breakPlatform)
            {
                breakPlatform = true;
                startTime = Time.time;
            }
        }
        
    }

}
