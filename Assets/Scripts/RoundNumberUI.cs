using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundNumberUI : MonoBehaviour
{
    [SerializeField] Text roundText;
    
    void Start()
    {
        roundText.text = "Round " + GameManager.Instance.RoundNumber;
    }
}
