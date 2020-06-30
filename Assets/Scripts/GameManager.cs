using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    int score = 0;
    [SerializeField] int maxScore = 100;
    [SerializeField] Board board;
    public int Score { get => score; set => score = value; }
    public int MaxScore { get => maxScore; set => maxScore = value; }

    public delegate void UpdateScoreEventHandler(int score);
    public event UpdateScoreEventHandler onScoreUpdate;

    private void Start()
    {
        board.onClear += AddScore;
    }

    void AddScore(int number)
    {
        Score += number;
        if(Score > MaxScore)
        {
            Score = MaxScore;
        }

        if(onScoreUpdate != null)
        {
            onScoreUpdate(Score);
        }
    }
}
