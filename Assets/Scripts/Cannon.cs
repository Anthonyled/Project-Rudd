using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject indicator;
    [SerializeField] float power;
    [SerializeField] float indicatorDist;
    [SerializeField] Renderer playerRenderer;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Rigidbody2D cannonRb;
    private float angle;
    private Rigidbody2D indicatorPos;
    private bool insideCannon;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        cannonRb = GetComponent<Rigidbody2D>();
        indicatorPos = indicator.GetComponent<Rigidbody2D>();
        insideCannon = false;
        playerController = player.GetComponent<PlayerController>();
        angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        indicatorPos.position = cannonRb.position + new Vector2(indicatorDist * Mathf.Cos(angle), indicatorDist * Mathf.Sin(angle));
        if (Input.GetKeyDown(KeyCode.Space) && insideCannon)
        {
            print("what's updog");
            rb.velocity = new Vector2(power * Mathf.Cos(angle), power * Mathf.Sin(angle));
        }

        if (Input.GetKeyDown(KeyCode.W) && insideCannon)
        {
            print("hi hello :3");
            angle += (float)0.1;
        }

        if (Input.GetKeyDown(KeyCode.S) && insideCannon)
        {
            print("hi pookie :3");
            angle -= (float)0.1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("i like rats");
            insideCannon = true;
            playerRenderer.enabled = false;
            playerController.lockMovement();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("you're so cool and awesome and also super smart");
            insideCannon = false;
            playerRenderer.enabled = true;
            playerController.unlockMovement();
        }
    }
}
