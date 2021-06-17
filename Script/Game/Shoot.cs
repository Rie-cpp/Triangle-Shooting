using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class Shoot : MonoBehaviour
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

        Destroy(gameObject, 3);
    }
}
