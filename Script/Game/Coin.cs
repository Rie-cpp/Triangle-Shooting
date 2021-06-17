using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //private int val = 1;
    public float CoinBrake = 0.1f;

    private Vector3 CoinDerection;
    private float CoinSpeed;

    // プレイヤーを追尾
    public float FollowAccel = 0.0001f;
    private bool isFollow;
    private float FollowSpeed;

    void Update()
    {
        var PlayerPos = PlayerMovement.Instance.transform.localPosition;
        var Distance = Vector3.Distance(PlayerPos, transform.localPosition);

        if (Distance < 0.7)
        {
            isFollow = true;
        }

        if (isFollow && PlayerMovement.Instance.gameObject.activeSelf)
        {
            var Direction = PlayerPos - transform.localPosition;
            Direction.Normalize();

            transform.localPosition += Direction * FollowSpeed;
            FollowSpeed += FollowAccel;
            return;
        }

        // 速度計算
        var velocity = CoinDerection * CoinSpeed;

        transform.localPosition += velocity;

        CoinSpeed *= CoinBrake;

        transform.localPosition = Utils.ClampPosition(transform.localPosition);
    }

    public void Init(int score, float speedMin, float speedMax)
    {
        var angle = Random.Range(0, 360);

        // 進行方向をradに変換
        var f = angle * Mathf.Deg2Rad;

        CoinDerection = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0);
        CoinSpeed = Mathf.Lerp(speedMin, speedMax, Random.value);
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Player")) return;

        Destroy(gameObject);

        var player = collision.GetComponent<PlayerMovement>();
        player.CoinCounter();
    }
}
