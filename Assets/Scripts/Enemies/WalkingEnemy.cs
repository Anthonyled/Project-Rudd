using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentPoint;
    private bool isFacingRight = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoint == pointB.transform) {
            rb.velocity = new Vector2(speed, 0);
        }
        else {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointB.transform) {
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointA.transform) {
            currentPoint = pointB.transform;
        }

        if (speed > 0 && !isFacingRight)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            isFacingRight = true;
        } 
        else if (speed < 0 && isFacingRight) 
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            isFacingRight = false;
        }
    }

}
