using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGauge : MonoBehaviour
{
    [SerializeField]
    public Image GreenGauge;

    void Update()
    {
        var player = PlayerMovement.Instance;

        var hp = player.PlayerHP;
        var hpMax = player.PlayerMaxHP;
        GreenGauge.fillAmount = (float)hp / hpMax;
    }
}
