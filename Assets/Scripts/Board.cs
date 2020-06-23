using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Board Size")]
    public int width;
    public int height;
    public int lateralSize;

    [Header("Tiles")]
    public GameObject tilePrefab;
    public Tile[,] tiles;

    [Header("Food")]
    public GameObject foodPrefab;
    public float swapMovementDuration = 1f;
    public Sprite[] foodSprites;

    void Start()
    {
        tiles = new Tile[width, height];
        SetupTiles();
        SetupCamera();
        StartCoroutine(InputUpdate());
    }

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity, transform);
                tile.name = i + "," + j;
                tiles[i,j] = tile.GetComponent<Tile>();
                tiles[i,j].Init(i, j, GenerateRandomFood());
            }
        }
    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float) (width - 1) / 2f, (float) (height - 1) / 2f, -10f);
        float aspectRatio = (float) Screen.width / (float) Screen.height;
        float verticalSize = (float)height / 2f + (float)lateralSize;
        float horizontalSize = ((float)width / 2f + (float)lateralSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }

    GameObject GenerateRandomFood()
    {
        GameObject food = Instantiate(foodPrefab, Vector3.zero, Quaternion.identity);
        int randomNumber = Random.Range(0, foodSprites.Length);
        food.GetComponent<Food>().Init(foodSprites[randomNumber], randomNumber, swapMovementDuration);
        return food;
    }

    IEnumerator InputUpdate()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield return SwapFood(tiles[1, 1], tiles[1, 2]);
            }
            yield return null;
        }
    }

    IEnumerator SwapFood(Tile tileA, Tile tileB)
    {
        GameObject foodToSwap = tileA.GetFood();
        StartCoroutine(tileA.PlaceFood(tileB.GetFood()));
        StartCoroutine(tileB.PlaceFood(foodToSwap));
        yield return new WaitUntil(() => !tileA.isPlacingFood && !tileB.isPlacingFood);
    }
}
