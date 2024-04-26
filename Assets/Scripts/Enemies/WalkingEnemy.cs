using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentPoint;
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
    }

    public float getVelocity()
    {
        return rb.velocity.x;
    }

}
