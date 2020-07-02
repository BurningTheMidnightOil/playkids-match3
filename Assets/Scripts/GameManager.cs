using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("General")]
    Board board;
    [SerializeField] GameObject nextRoundButton;
    [SerializeField] GameObject retryRoundButton;
    [SerializeField] GameObject canvasOverlay;
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
    public Board Board { get => board; 
        set 
        {
            board = value;
            board.onClear += AddScore;
        } 
    }

    public delegate void UpdateScoreEventHandler(int score);
    public event UpdateScoreEventHandler onScoreUpdate;

    private void Start()
    {
        DontDestroyOnLoad(canvasOverlay);
    }

    void AddScore(int number)
    {
        Score += number;
        if (Score > MaxScore)
        {
            Score = MaxScore;
            ShowNextRoundButton();
        }

        if (onScoreUpdate != null)
        {
            onScoreUpdate(Score);
        }
    }

    void ShowNextRoundButton()
    {
        if (!retryRoundButton.activeSelf) nextRoundButton.SetActive(true);
    }

    public void ShowRetryRoundButton()
    {
        if(!nextRoundButton.activeSelf) retryRoundButton.SetActive(true);
    }

    public void NextRound()
    {
        StartCoroutine(NextRoundCoroutine());
    }

    IEnumerator NextRoundCoroutine()
    {
        MaxScore += MaxScorePerRound;
        RoundNumber++;
        yield return CanvasOverlayUI.Instance.ResetNextButtonImage();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryRound()
    {
        StartCoroutine(RetryRoundCoroutine());
    }

    IEnumerator RetryRoundCoroutine()
    {
        yield return CanvasOverlayUI.Instance.ResetRetryButtonImage();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
