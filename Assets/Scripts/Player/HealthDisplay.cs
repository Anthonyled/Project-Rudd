using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] heartList;
    Health h;
    // Start is called before the first frame update
    void Start()
    {
        h = player.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(-0.6f, 2.5f, 0);
        for (int i = 0; i < h.getHealth(); i++) {
            heartList[i].SetActive(true);
        }
        for (int i = h.getMaxHealth() - 1; i >= h.getHealth(); i--) {
            heartList[i].SetActive(false);
        }
    }
}
