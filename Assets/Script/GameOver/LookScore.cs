using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookScore : MonoBehaviour
{
    public Text ScoreText;
    int Score;
    
    void Start()
    {
        Score = PlayerMovement.GetScore();
        ScoreText.text = string.Format("{0}", Score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
