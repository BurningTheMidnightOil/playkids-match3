using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Text scoreText;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "" + score;
        GameManager.Instance.onScoreUpdate += UpdateTextScore;
    }

    void UpdateTextScore(int score)
    {
        scoreText.text = "" + score;
    }
}
