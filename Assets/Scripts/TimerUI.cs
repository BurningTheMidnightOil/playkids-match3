using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] Text timerText;
    int seconds;
    int minutes;
    
    void Start()
    {
        seconds = GameManager.Instance.InitialSeconds;
        minutes = GameManager.Instance.InitialMinutes;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while(!(seconds == 0 && minutes == 0))
        {
            yield return new WaitForSeconds(1f);
            if(seconds > 0)
            {
                seconds--;
            }
            else if(minutes > 0)
            {
                minutes--;
                seconds = 59;
            }

            if(seconds > 10)
            {
                timerText.text = minutes + ":" + seconds;
            }
            else
            {
                timerText.text = minutes + ":0" + seconds;
            }
        }
        GameManager.Instance.ShowRetryRoundButton();
    }
}
