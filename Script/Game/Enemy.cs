using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using System.Linq;


public class Enemy : Utils
{
    private Transform Target;
    private float rad;
    

    private float Distance; // プレイヤーと敵との距離
    public int desiredDistance;

    public float EnemySpeed;
    public int EnemyHpMax;
    public int EnemyExp;

    private int EnemyHp;

    public EnemyShoot Shoot;

    public float ShootSpeed;
    public float ShootRange;
    public int ShootCount;
    public float ShootInterval;

    private float CurrentTime = 0;

    public Coin CoinPrefabs;
    public float CoinSpeedMin;
    public float CoinSpeedMax;

    public Explosion ExplosionPrefab;
    public Explosion ShootExplosionPrefab;

    public AudioClip Explosion;
    public AudioClip ShootSE;

    // Start is called before the first frame update
    void Start()
    {
        Target = PlayerMovement.Instance.transform;

        EnemyHp = EnemyHpMax;
    }

    // Update is called once per frame
    void Update()
    {
        rad = Mathf.Atan2(Target.position.y - transform.position.y,
                            Target.position.x - transform.position.x);

        if (Target == null) return;

        Vector2 TargetPosition = Target.position;
        Vector2 Position = transform.position;
        Distance = Vector2.Distance(TargetPosition, Position);

        if (Distance > desiredDistance)
        {
            Position.x += EnemySpeed * Mathf.Cos(rad);
            Position.y += EnemySpeed * Mathf.Sin(rad);

            transform.position = Position;

        }

        // enemyがプレイヤーの方向を向くようにする
        var direction = Target.position - transform.position;
        var angle = Utils.Aim(Vector3.zero, direction);

        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        CurrentTime += Time.deltaTime;

        // 弾を撃つ
        if (ShootInterval < CurrentTime)
        {
            CurrentTime = 0;
            ShootWay(angle, ShootRange, ShootSpeed, ShootCount);
            AudioSource.PlayClipAtPoint(ShootSE, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Shoot"))
        {
            Instantiate(ShootExplosionPrefab, collision.transform.localPosition, Quaternion.identity);

            GetComponents<AudioSource>()[0].Play();


            Destroy(collision.gameObject);

            EnemyHp--;

            if (EnemyHp > 0) return;

            Instantiate(ExplosionPrefab, collision.transform.localPosition, Quaternion.identity);

            Destroy(gameObject);

            AudioSource.PlayClipAtPoint(Explosion, transform.position);

            var exp = EnemyExp;

            while (exp > 0)
            {
                var CoinPrefab = CoinPrefabs;
                var coin = Instantiate(CoinPrefab, transform.localPosition, Quaternion.identity);
                coin.Init(EnemyExp, CoinSpeedMin, CoinSpeedMax);

                exp--;
                //UnityEngine.Debug.Log(exp);
            }
        }
    }

    private void ShootWay(float angleBase, float angleRange, float speed, int count)
    {
        var pos = transform.localPosition; // プレイヤーの位置
        var rot = transform.localRotation; // プレイヤーの向き

        // 弾を複数発射する場合
        if (1 < count)
        {
            // 発射する回数分ループする
            for (int i = 0; i < count; ++i)
            {
                // 弾の発射角度を計算する
                var angle = angleBase + angleRange * ((float)i / (count - 1) - 0.5f);

                // 発射する弾を生成する
                var shot = Instantiate(Shoot, pos, rot);

                // 弾を発射する方向と速さを設定する
                shot.Init(angle, speed);
            }
        }
        // 弾を 1 つだけ発射する場合
        else if (count == 1)
        {
            // 発射する弾を生成する
            var shot = Instantiate(Shoot, pos, rot);

            // 弾を発射する方向と速さを設定する
            shot.Init(angleBase, speed);
        }
    }
}
