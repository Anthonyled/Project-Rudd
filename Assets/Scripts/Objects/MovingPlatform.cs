using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] List<Vector2> moveCoords;
    [SerializeField] int platformMoveSpeed;
    private int i = 0;


    private void Start()
    {
        InvokeRepeating(nameof(Move), platformMoveSpeed, platformMoveSpeed);
    }

    private void Move()
    {
        StartCoroutine(MoveToPosition(moveCoords[i % moveCoords.Count], platformMoveSpeed));
        i++;
    }

    public IEnumerator MoveToPosition(Vector2 end, float sec)
    {
        float elapsedTime = 0;
        Vector2 startingPos = transform.position;
        while (elapsedTime < sec)
        {
            transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / sec));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
    }
}
