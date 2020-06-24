using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public GameObject food;
    public bool isPlacingFood = false;

    public delegate void MouseEventHandler(Tile tile);
    public event MouseEventHandler onMouseDown;
    public event MouseEventHandler onMouseEnter;
    public event MouseEventHandler onMouseUp;

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

    private void OnMouseDown()
    {
        if(onMouseDown != null)
        {
            onMouseDown(this);
        }
    }

    private void OnMouseEnter()
    {
        if (onMouseEnter != null)
        {
            onMouseEnter(this);
        }
    }

    private void OnMouseUp()
    {
        if (onMouseUp != null)
        {
            onMouseUp(this);
        }
    }
}
