using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;


public class RushEnemy : Utils
{
    private Transform Target;
    private float rad;

    private float Distance; // プレイヤーと敵との距離

    public float EnemySpeed;
    public int EnemyHpMax;
    public int EnemyExp;

    private int EnemyHp;

    public Coin CoinPrefabs;
    public float CoinSpeedMin;
    public float CoinSpeedMax;

    public Explosion ExplosionPrefab;

    public AudioClip Explosion;

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

        Position.x += EnemySpeed * Mathf.Cos(rad);
        Position.y += EnemySpeed * Mathf.Sin(rad);

        transform.position = Position;

        // enemyがプレイヤーの方向を向くようにする
        var direction = Target.position - transform.position;
        var angle = Utils.Aim(Vector3.zero, direction);

        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("Shoot"))
        {
            GetComponents<AudioSource>()[0].Play();

            Destroy(collision.gameObject);

            EnemyHp--;

            if (EnemyHp > 0) return;

            Instantiate(ExplosionPrefab, collision.transform.localPosition, Quaternion.identity);

            AudioSource.PlayClipAtPoint(Explosion, transform.position);

            Destroy(gameObject);

            var exp = EnemyExp;

            while (exp > 0)
            {
                var coin = Instantiate(CoinPrefabs, transform.localPosition, Quaternion.identity);
                coin.Init(EnemyExp, CoinSpeedMin, CoinSpeedMax);

                exp--;
            }
        }
    }
}