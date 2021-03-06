﻿using System.Collections;
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
        SetFood(food);
    }

    public void SetFood(GameObject food)
    {
        food.transform.parent = transform;
        this.food = food;
    }

    public GameObject GetFood()
    {
        return food;
    }

    public int GetTypeOfFood()
    {
        return food.GetComponent<Food>().foodNumber;
    }

    public void RemoveFood()
    {
        Destroy(food);
    }

    public void DettachFood(Food foodToDettach)
    {
        if(food == foodToDettach) food = null;
    }
    public IEnumerator PlaceFood(GameObject foodObject, float duration)
    {
        isPlacingFood = true;
        if(foodObject != null)
        {
            food = foodObject;
            yield return foodObject.GetComponent<Food>().MoveTo(gameObject, duration);
        } 
        else
        {
            food = null;
        }
        
        isPlacingFood = false;
    }

    private void OnMouseDown()
    {
        food.GetComponent<Food>().ChangeSpriteToSelected();
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
        food.GetComponent<Food>().ChangeSpriteToUnselected();
        if (onMouseUp != null)
        {
            onMouseUp(this);
        }
    }
}
