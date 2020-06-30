using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("General")]
    [SerializeField] Board board;
    int roundNumber = 1;

    [Header("Score")]
    int score = 0;
    [SerializeField] int maxScore = 100;
    [SerializeField] int maxScorePerRound = 20;

    [Header("Timer")]
    [SerializeField] int initialMinutes = 2;
    [SerializeField] int initialSeconds = 0;

    public int Score { get => score; set => score = value; }
    public int MaxScore { get => maxScore; set => maxScore = value; }
    public int InitialMinutes { get => initialMinutes; set => initialMinutes = value; }
    public int InitialSeconds { get => initialSeconds; set => initialSeconds = value; }
    public int MaxScorePerRound { get => maxScorePerRound; set => maxScorePerRound = value; }
    public int RoundNumber { get => roundNumber; set => roundNumber = value; }

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
            FinishRound();
        }

        if(onScoreUpdate != null)
        {
            onScoreUpdate(Score);
        }
    }

    void FinishRound()
    {
        MaxScore += MaxScorePerRound;
        RoundNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
