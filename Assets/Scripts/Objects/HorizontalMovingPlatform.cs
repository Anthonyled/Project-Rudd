using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    PlayerController playerController;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < pointA.transform.position.x)
        {
            rb.velocity = new Vector2(speed, 0);
            playerController.MovingPlatformChange(speed, this);
        }
        if (transform.position.x > pointB.transform.position.x)
        {
            rb.velocity = new Vector2(-speed, 0);
            playerController.MovingPlatformChange(-speed, this);
        }
    }

}
