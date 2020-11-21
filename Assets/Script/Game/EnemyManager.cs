using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager instance
    {
        get
        {
            _instance = FindObjectOfType<EnemyManager>();
            return _instance;
        }
    }

    public GameObject EnemyPrefab;
    // enemyの初期位置
    Vector2 originalPos;
    // enemyの初期角度
    Quaternion originalRot;
    private List<GameObject> pooledGameObjects;
    private int enemyNum = 0;

    public float EnemyIntervalFrom;
    public float EnemyIntervalTo;
    public float ElapsedTimeMax;
    public float ElapsedTime;

    private float EnemyTimer;

    private void Start()
    {
        pooledGameObjects = new List<GameObject>();
    }

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
        GetEnemy();
    }

    public GameObject GetEnemy()
    {
        float x = Random.Range(-8, 8);
        float y = Random.Range(-4, 4);
        originalPos = new Vector2(x, y);

        // poolされたゲームオブジェクトで使用できるものがある場合
        foreach (GameObject obj in pooledGameObjects)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.transform.position = originalPos;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                return obj;
            }
        }
        // 使用できるものがなかった場合
        GameObject newObj = (GameObject)Instantiate(EnemyPrefab, originalPos, transform.rotation);
        // enemyに番号をつける
        newObj.gameObject.name = "Enemy" + enemyNum.ToString();
        enemyNum++;
        // リストに追加
        pooledGameObjects.Add(newObj);
        return newObj;
    }

    public void releaseEnemy(GameObject obj)
    {
        obj.SetActive(false);
    }
}
