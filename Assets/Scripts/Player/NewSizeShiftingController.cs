// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NewSizeShiftingController : MonoBehaviour
// {
//     private int currScale = 0; // -1 == small, 0 == normal, 1 == big
//     private bool canScale = true;
//     private Rigidbody2D rb;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     private void OnChangeBigger()
//     {
//         if (currScale != 1 && canScale)
//         {
//             canScale = false;
//             StartCoroutine(ScaleAnimation(1, 2f));
//             speed *= 1.5f;
//             currScale++;
//         }
//     }

//     private void OnChangeSmaller()
//     {
//         if (currScale != -1 && canScale)
//         {
//             canScale = false;
//             StartCoroutine(ScaleAnimation(1, 0.5f));
//             currScale--;
//             speed /= 1.5f;
//         }
//     }

//     IEnumerator ScaleAnimation(float time, float scale)
//     {
//         Invoke(nameof(UnlockScaling), time);
//         float i = 0;
//         float rate = 1 / time;
//         Vector2 fromScale = transform.localScale;
//         Vector2 toScale = transform.localScale * scale;

//         float startingMass = rb.mass;
//         float startingGravityScale = rb.gravityScale;
//         while (i < 1)
//         {
//             i += Time.deltaTime * rate; // i is on a scale from 0 to 1, with 0 being the start of the animation and 1 being the end
//             Vector2 p = rb.mass * rb.velocity; // What does this do? -Zach
            
//             Vector2 newScale = Vector2.Lerp(fromScale, toScale, i); // Lerp does a linear scale from the start to end
//             if (!isFacingRight)
//             {
//                 newScale.x *= -1; // Face left
//             }
//             transform.localScale = newScale;
//             rb.mass = Mathf.Lerp(startingMass, startingMass * scale, i);
//             rb.velocity = p / rb.mass; // What does this do? -Zach

//             rb.gravityScale = Mathf.Lerp(startingGravityScale, startingGravityScale * scale, i); // Scale gravity
//             yield return 0;
//         }
//     }

//     private void UnlockScaling()
//     {
//         canScale = true;
//     }
// }
