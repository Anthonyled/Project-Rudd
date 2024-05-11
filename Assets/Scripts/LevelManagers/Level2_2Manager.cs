using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Level2_2Manager : MonoBehaviour
{
    private int keyFragmentsObtained = 0;
    private int keyFragmentsNeeded = 2;

    [SerializeField] private UnityEvent openDoor;
    [SerializeField] private UnityEvent openDoor2;
    public void incrementKeyFragments ()
    {
        keyFragmentsObtained++;
        if (keyFragmentsNeeded == keyFragmentsObtained)
        {
            if (keyFragmentsNeeded == 2)
            {
                openDoor?.Invoke();
            } else
            {
                openDoor2?.Invoke();
            }
        }
    }

    public void setKeyFragmentsNeeded(int n)
    {
        keyFragmentsNeeded = n;
        keyFragmentsObtained = 0;
    }
}
