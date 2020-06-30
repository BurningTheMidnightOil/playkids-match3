using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    int score = 0;

    [SerializeField] Board board;

    public delegate void UpdateScoreEventHandler(int score);
    public event UpdateScoreEventHandler onScoreUpdate;

    private void Start()
    {
        board.onClear += AddScore;
    }

    void AddScore(int number)
    {
        score += number;
        if(onScoreUpdate != null)
        {
            onScoreUpdate(score);
        }
    }
}
