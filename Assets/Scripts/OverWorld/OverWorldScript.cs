using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OverWorldScript : MonoBehaviour
{

    [SerializeField] LevelNode startNode;
    PlayerController pc;
    LevelNode currNode;
    Animator animator;
    bool isFacingRight = true;
    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        currNode = startNode;
        transform.position = startNode.pos;
        animator = GetComponent<Animator>();
        startNode.locked = false;
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        if (xAxis > 0) { moveSelector("right"); }
        if (xAxis < 0) { moveSelector("left"); }
        if (yAxis > 0) { moveSelector("up"); }
        if (yAxis < 0) { moveSelector("down"); }

        if (LevelManager.levelTracker.ContainsKey(currNode.levelName))
        {
            if (currNode.right != null)
                currNode.right.locked = false;
            if (currNode.left != null)
                currNode.left.locked = false;
            if (currNode.up != null)
                currNode.up.locked = false;
            if (currNode.down != null)
                currNode.down.locked = false;
        }
    }

    void moveSelector(string dir)
    {
        if (dir == "right")
        { 
            if (currNode.right != null && !currNode.right.locked && !isRunning)
            {
                if (!isFacingRight)
                {
                    FlipSprite();
                }
                StartCoroutine(MoveToPosition(currNode.right.pos, 1));
                currNode = currNode.right;
            }
        }
        else if (dir == "left")
        {
            if (currNode.left != null && !currNode.left.locked && !isRunning)
            {
                if (isFacingRight)
                {
                    FlipSprite();
                }
                StartCoroutine(MoveToPosition(currNode.left.pos, 1));
                currNode = currNode.left;
            }
        }
        else if (dir == "up")
        {
            if (currNode.up != null && !currNode.up.locked && !isRunning)
            {
                StartCoroutine(MoveToPosition(currNode.up.pos, 1));
                currNode = currNode.up;
            }
        }
        else
        {
            if (currNode.down != null && !currNode.down.locked && !isRunning)
            {
                StartCoroutine(MoveToPosition(currNode.down.pos, 1));
                currNode = currNode.down;
            }
        }
    }

    public IEnumerator MoveToPosition(Vector2 end, float sec)
    {
        isRunning = true;
        animator.SetBool("onMove", true);
        float elapsedTime = 0;
        Vector2 startingPos = transform.position;
        while (elapsedTime < sec)
        {
            transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / sec));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
        animator.SetBool("onMove", false);
        isRunning = false;
    }

    void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    public void OnEnterBuilding(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(currNode.levelName);
    }
}
