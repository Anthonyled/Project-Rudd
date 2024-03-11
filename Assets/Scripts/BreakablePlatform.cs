using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    [SerializeField] float timeToBreak;
    [SerializeField] int timeToRespawn;
    [SerializeField] PlayerController player;
    bool breakPlatform = false;
    float startTime;

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!breakPlatform && player.GetSize() != PlayerController.size.Small)
            {
                Invoke(nameof(BreakPlatform), timeToBreak);
                breakPlatform = true;
            }
        }
    }

    private void BreakPlatform()
    {
        gameObject.SetActive(false);
        Invoke(nameof(RespawnPlatform), timeToRespawn);
    }

    private void RespawnPlatform()
    {
        gameObject.SetActive(true);
        breakPlatform = false;
    }

}
