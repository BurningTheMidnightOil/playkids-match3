using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int foodNumber;

    public void Init(Sprite sprite, int number)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        foodNumber = number;
    }
}
