using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Level2_2Manager : MonoBehaviour
{
    private int keyFragmentsObtained = 0;
    private int keyFragmentsNeeded = 2;

    [SerializeField]  private UnityEvent openDoor;
    public void incrementKeyFragments ()
    {
        keyFragmentsObtained++;
        if (keyFragmentsNeeded == keyFragmentsObtained)
        {
            openDoor?.Invoke();
        }
    }

    public void setKeyFragmentsNeeded(int n)
    {
        keyFragmentsNeeded = n;
    }
}
