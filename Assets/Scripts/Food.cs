using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int foodNumber;
    public delegate void MovingEvent(Food food);

    public void Init(Sprite sprite, int foodNumber)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        this.foodNumber = foodNumber;
    }

    public IEnumerator MoveTo(GameObject tile, float movementDuration)
    {
        transform.parent = tile.transform;
        Vector3 startPosition = transform.localPosition;
        Vector3 destination = Vector3.zero;
        bool reachedDestination = false;
        float elapsedTime = 0f;
        float t;

        while(!reachedDestination)
        {
            if(Vector3.Distance(transform.localPosition, destination) < 0.01f)
            {
                reachedDestination = true;
                transform.localPosition = destination;
                break;
            }

            elapsedTime += Time.deltaTime;
            t = Mathf.Clamp(elapsedTime / movementDuration, 0f, 1f);

            transform.localPosition = Vector3.Lerp(startPosition, destination, t);

            yield return null;
        }
    }
}
