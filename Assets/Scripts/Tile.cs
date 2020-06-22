using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public Food food;

    public void Init(int x, int y, GameObject foodObject)
    {
        xIndex = x;
        yIndex = y;

        //Set Food
        food = foodObject.GetComponent<Food>();
        foodObject.transform.parent = transform;
        foodObject.transform.localPosition = Vector3.zero;
    }
}
