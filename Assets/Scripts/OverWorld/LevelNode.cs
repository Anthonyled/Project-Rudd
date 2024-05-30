using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNode : MonoBehaviour
{
    [SerializeField] public LevelNode left;
    [SerializeField] public LevelNode right;
    [SerializeField] public LevelNode up;
    [SerializeField] public LevelNode down;
    public Vector2 pos;

    [SerializeField] Sprite grayscaled;
    [SerializeField] Sprite original;

    SpriteRenderer renderer;
    public bool locked = true;
    [SerializeField] public string levelName;
    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = grayscaled;
        pos = transform.position - new Vector3(0, 1, 0);
    }

    public void Update()
    {
        if (!locked)
        {
            renderer.sprite = original;
        }
    }
}
