using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public GameObject food;
    public bool isPlacingFood = false;

    public void Init(int x, int y, GameObject food)
    {
        xIndex = x;
        yIndex = y;
        StartCoroutine(PlaceFood(food));
    }

    public GameObject GetFood()
    {
        return food;
    }
    public IEnumerator PlaceFood(GameObject foodObject)
    {
        isPlacingFood = true;
        food = foodObject;
        yield return foodObject.GetComponent<Food>().MoveTo(gameObject);
        isPlacingFood = false;
    }
}
