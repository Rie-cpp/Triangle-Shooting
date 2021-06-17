using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public GameObject EnemyPrefab;

    public float EnemyIntervalFrom;
    public float EnemyIntervalTo;
    public float ElapsedTimeMax;
    public float ElapsedTime;

    private float EnemyTimer;


    // Update is called once per frame
    void Update()
    {
        //経過時間
        ElapsedTime += Time.deltaTime;
        //出現するタイミング
        EnemyTimer += Time.deltaTime;

        // ゲームの経過時間から出現間隔を算出する　どんどん短くなる
        var t = ElapsedTime / ElapsedTimeMax;
        var interval = Mathf.Lerp(EnemyIntervalFrom, EnemyIntervalTo, t);

        if (EnemyTimer < interval) return;

        EnemyTimer = 0;
        SpawnObject();
    }

    void SpawnObject()
    {
        float x = Random.Range(-8, 8);
        float y = Random.Range(-4, 4);
        transform.position = new Vector2(x, y);
        Instantiate(EnemyPrefab, transform.position, transform.rotation);
    }
}
