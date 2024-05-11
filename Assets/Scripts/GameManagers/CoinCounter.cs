using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;

    public TMP_Text coinText;
    public int coins = 0;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "Coins: " + coins.ToString();
    }

    public void increaseCoins() {
        coins++;
        coinText.text = "Coins: " + coins.ToString();
    }
}
