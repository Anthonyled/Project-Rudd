using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] int effectiveDistance;
    bool inEffect = false;

    void Update()
    {
        if (player.GetPosition().y > transform.position.y - 0.5 && player.GetPosition().y < transform.position.y + 0.5 &&
            player.GetPosition().x - transform.position.x < effectiveDistance / player.GetMass() && player.GetPosition().x - transform.position.x > 0)
        {
            player.SetVelocity(new Vector2(10, Mathf.Max(0, player.GetVelocity().y)));
            player.lockMovement = true;
            inEffect = true;
        }
        else
        {
            if (inEffect)
            {
                player.lockMovement = false;
                player.SetVelocity(new Vector2(0, 0));
                inEffect = false;
            }
        }
    }
}