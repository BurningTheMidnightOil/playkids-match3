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
    public Tile[,] allTiles;

    [Header("Food")]
    public GameObject foodPrefab;
    public Sprite[] foodSprites;

    void Start()
    {
        allTiles = new Tile[width, height];
        SetupTiles();
        SetupCamera();
    }

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity, transform);
                tile.name = i + "," + j;
                allTiles[i,j] = tile.GetComponent<Tile>();
                GameObject food = GenerateRandomFood();
                allTiles[i, j].Init(i, j, food);
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
        food.GetComponent<Food>().Init(foodSprites[randomNumber], randomNumber);
        return food;
    }
}
