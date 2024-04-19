using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager3_1 : MonoBehaviour
{
    [SerializeField] DoorScript door;
    private void Start()
    {
        door.Open();
    }
}
