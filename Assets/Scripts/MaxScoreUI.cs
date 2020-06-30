using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxScoreUI : MonoBehaviour
{
    [SerializeField] Text maxScoreText;
    [SerializeField] Slider slider;

    void Start()
    {
        maxScoreText.text = "" + GameManager.Instance.MaxScore;
        GameManager.Instance.onScoreUpdate += UpdateSlider;
    }

    void UpdateSlider(int score)
    {
        slider.value = (float) score / (float) GameManager.Instance.MaxScore;
    }
}
