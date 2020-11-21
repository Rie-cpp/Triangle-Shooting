using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Vector3 ShootSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += ShootSpeed;
    }

    public void Init(float angle, float speed)
    {
        // 弾の発射角度をベクトルに変換する
        var direction = Utils.GetDirection(angle);

        // 発射角度と速さから速度を求める
        ShootSpeed = direction * speed;

        // 弾が進行方向を向く
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        Destroy(gameObject, 8);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Player")) return;

        Destroy(gameObject);

        var player = collision.GetComponent<PlayerMovement>();
        player.Damage(20);
        Debug.Log(player.PlayerHP);
    }
}
