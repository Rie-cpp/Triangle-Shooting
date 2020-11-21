using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : Utils
{
    public float speed;
    public float PlayerHP;
    public float PlayerMaxHP;

    public Shoot ShootPrefab;
    public float ShootSpeed;
    public float ShootRange;
    private float ShootTimer = 0;
    public int ShootCount;
    public float ShootInterval;

    public int CoinCount;
    public static int Score = 0;

    public Explosion ExplosionPrefab;
    public Explosion ShootExplosionPrefab;

    public AudioClip ShootSE;
    public AudioClip ExplosionSE;
    public AudioClip CoinSE;

    public static PlayerMovement Instance { get; set; }

    private void Start()
    {
        Instance = this;
        PlayerHP = PlayerMaxHP;
    }

    void Update()
    {
        PlayerMove();
        // プレイヤーが画面外に出ないようにする
        transform.localPosition = Utils.ClampPosition(transform.localPosition);

        // プレイヤーの向きについて
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);
        var direction = Input.mousePosition - screenPos;
        var angle = Utils.Aim(Vector3.zero, direction);

        // look mouse
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        // 弾の発射
        ShootTimer += Time.deltaTime;

        if (ShootTimer < ShootInterval) return;

        if (Input.GetMouseButton(0))
        {
            ShootTimer = 0;
            ShootWay(angle, ShootRange, ShootSpeed, ShootCount);
            AudioSource.PlayClipAtPoint(ShootSE, transform.position);
        }

        
    }

    private void PlayerMove()
    {
        // 水平方向の移動量
        float x = Input.GetAxis("Horizontal");
        // 垂直方向の移動量
        float y = Input.GetAxis("Vertical");

        var velocity = new Vector3(x, y) * speed;
        transform.localPosition += velocity;
    }

    // 弾を発射する関数
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
                var shot = Instantiate(ShootPrefab, pos, rot);

                // 弾を発射する方向と速さを設定する
                shot.Init(angle, speed);
            }
        }
        // 弾を 1 つだけ発射する場合
        else if (count == 1)
        {
            // 発射する弾を生成する
            var shot = Instantiate(ShootPrefab, pos, rot);

            // 弾を発射する方向と速さを設定する
            shot.Init(angleBase, speed);
        }
    }

    public void Damage(int damage)
    {
        AudioSource.PlayClipAtPoint(ExplosionSE, transform.position);

        PlayerHP -= damage;

        // ゲームオーバー
        if (PlayerHP > 0) return;
        Instantiate(ExplosionPrefab, transform.localPosition, Quaternion.identity);
        gameObject.SetActive(false);
        SceneManager.LoadScene("GameOverScene");
    }

    public static int GetScore()
    {
        return Score;
    }

    public void CoinCounter()
    {
        AudioSource.PlayClipAtPoint(CoinSE, transform.position);
        CoinCount++;
        Score = CoinCount;
        Debug.Log(CoinCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(ShootExplosionPrefab, collision.transform.localPosition, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Damage(10);
        }
    }
}