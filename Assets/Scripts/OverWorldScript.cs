using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldScript : MonoBehaviour
{
    internal class Node
    {
        internal Vector2 data;
        internal Node left;
        internal Node right;
        internal Node up;
        internal Node down;

        public Node(Vector2 d)
        {
            data = d;
            left = null;
            right = null;
            up = null;
            down = null;
        }
    }

    Node currNode;

    // Start is called before the first frame update
    void Start()
    {
        currNode = new Node(new Vector2(0,0));
        currNode.right = new Node(new Vector2(1,0));
        currNode.right.left = currNode;
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
    }

    void moveSelector(string dir)
    {
        if (dir == "right")
        {
            if (currNode.right != null)
            {
                transform.position = currNode.right.data;
                currNode = currNode.right;
            }
        }
        else if (dir == "left")
        {
            if (currNode.left != null)
            {
                transform.position = currNode.left.data;
                currNode = currNode.left;
            }
        }
        else if (dir == "up")
        {

            if (currNode.up != null)
            {
                transform.position = currNode.up.data;
                currNode = currNode.up;
            }
        }
        else
        {
            if (currNode.down != null)
            {
                transform.position = currNode.down.data;
                currNode = currNode.down;
            }
        }
    }
}
