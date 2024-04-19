/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VerticalFan : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] int effectiveDistance;
    bool inEffect = false;

    void Update()
    {
        if (player.GetPosition().x > transform.position.x - 0.5 && player.GetPosition().x < transform.position.x + 0.5 &&
            player.GetPosition().y - transform.position.y < effectiveDistance / player.GetMass() && player.GetPosition().y - transform.position.y > 0)
        {
            player.SetVelocity(new Vector2(Mathf.Max(0, player.GetVelocity().x), 10));
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
}*/