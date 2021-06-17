using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCount : MonoBehaviour
{
    public Text Coin;

    // Update is called once per frame
    void Update()
    {
        var player = PlayerMovement.Instance;

        Coin.text = player.CoinCount.ToString(); 
    }
}
