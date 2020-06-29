using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] int initialMinutes;
    [SerializeField] int initialSeconds;
    
    void Start()
    {
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while(!(initialSeconds == 0 && initialMinutes == 0))
        {
            yield return new WaitForSeconds(1f);
            if(initialSeconds > 0)
            {
                initialSeconds--;
            }
            else if(initialMinutes > 0)
            {
                initialMinutes--;
                initialSeconds = 59;
            }

            if(initialSeconds > 10)
            {
                timerText.text = initialMinutes + ":" + initialSeconds;
            }
            else
            {
                timerText.text = initialMinutes + ":0" + initialSeconds;
            }
        }
    }
}
