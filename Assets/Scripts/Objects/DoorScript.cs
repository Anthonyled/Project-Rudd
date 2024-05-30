using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetTrigger("DoorOpen");
    }

    public void Close()
    {
        animator.SetTrigger("DoorClose");
    }
}
