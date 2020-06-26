using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Tile clickedTile;
    Tile targetedTile;
    bool isSwappingTiles = false;

    [Header("Food")]
    public GameObject foodPrefab;
    public float swapMovementDuration = 1f;
    public float setupPlaceFoodDuration = 0.1f;
    public Sprite[] foodSprites;

    void Start()
    {
        tiles = new Tile[width, height];
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
                tiles[i,j] = tile.GetComponent<Tile>();
                tiles[i,j].Init(i, j, GenerateRandomFood((float) i, (float) j), setupPlaceFoodDuration);
                tiles[i, j].onMouseDown += TileClicked;
                tiles[i, j].onMouseEnter += TileTargeted;
                tiles[i, j].onMouseUp += TileSwap;
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

    GameObject GenerateRandomFood(float x, float y)
    {
        GameObject food = Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);
        int randomNumber = Random.Range(0, foodSprites.Length);
        food.GetComponent<Food>().Init(foodSprites[randomNumber], randomNumber);
        return food;
    }

    void TileClicked(Tile tile)
    {
        clickedTile = tile;
    }

    void TileTargeted(Tile tile)
    {
        if (clickedTile != null && clickedTile != tile && isAdjacent(clickedTile, tile))
        {
            targetedTile = tile;
        } else
        {
            targetedTile = null;
        }
    }

    void TileSwap(Tile tile)
    {
        if (clickedTile != null && targetedTile != null && !isSwappingTiles)
        {
            StartCoroutine(PlayerSwapFoodCommand(clickedTile, targetedTile));
            clickedTile = null;
            targetedTile = null;
        }
    }
    IEnumerator PlayerSwapFoodCommand(Tile tileA, Tile tileB)
    {
        isSwappingTiles = true;

        yield return SwapFood(tileA, tileB, swapMovementDuration);
        FindMatchesAndRemoveFromBoardAtTiles(tileA, tileB);
        yield return FillEmptyTiles();
        bool foundMatches = false;

        do
        {
            foundMatches = SearchForMatches();
            yield return FillEmptyTiles();
        }
        while (foundMatches);

        isSwappingTiles = false;
    }

    IEnumerator SwapFood(Tile tileA, Tile tileB, float duration)
    {
        GameObject foodToSwap = tileA.GetFood();
        StartCoroutine(tileA.PlaceFood(tileB.GetFood(), duration));
        StartCoroutine(tileB.PlaceFood(foodToSwap, duration));
        yield return new WaitUntil(() => !tileA.isPlacingFood && !tileB.isPlacingFood);
    }

    void FindMatchesAndRemoveFromBoardAtTiles(Tile tileA, Tile tileB)
    {
        var combinedMatches = FindMatchesAt(tileA).Union(FindMatchesAt(tileB)).ToList();
        foreach (Tile matchedTile in combinedMatches)
        {
            matchedTile.RemoveFood();
        }
    }

    IEnumerator FillEmptyTiles()
    {
        yield return null; //For the destroyed objects to update
        for(int i = 0; i < width; i++)
        {
            bool hasEmptyTile = false;
            do
            {
                hasEmptyTile = false;
                for (int j = 0; j < height; j++)
                {
                    if (tiles[i, j].GetFood() == null)
                    {
                        hasEmptyTile = true;
                        if (j + 1 >= height)
                        {
                            yield return tiles[i, j].PlaceFood(GenerateRandomFood((float) i, (float) height), 0.02f);
                        } 
                        else if (tiles[i, j + 1].GetFood() != null)
                        {
                            yield return SwapFood(tiles[i, j + 1], tiles[i, j], 0.02f);
                        }
                    }
                }
            } while (hasEmptyTile);
        }
        yield return null;
    }

    bool SearchForMatches()
    {
        List<Tile> horizontalMatches = new List<Tile>();
        for (int i = 0; i < width; i++)
        {
            horizontalMatches = horizontalMatches.Union(FindColumnMatches(i)).ToList();
        }

        List<Tile> verticalMatches = new List<Tile>();
        for (int j = 0; j < height; j++)
        {
            verticalMatches = verticalMatches.Union(FindRowMatches(j)).ToList();
        }

        var combinedMatches = horizontalMatches.Union(verticalMatches).ToList();

        foreach (Tile matchedTile in combinedMatches)
        {
            matchedTile.RemoveFood();
        }
        return combinedMatches.Count > 0;
    }
    
    bool isAdjacent(Tile tileA, Tile tileB)
    {
        return Mathf.Abs(tileA.xIndex - tileB.xIndex) + Mathf.Abs(tileA.yIndex - tileB.yIndex) == 1;
    }

    List<Tile> FindMatchesAt(Tile tile)
    {
        int foodType = tile.GetTypeOfFood();

        List<Tile> horizontalMatchList = new List<Tile>();
        horizontalMatchList.Add(tile);

        for(int i = tile.xIndex - 1;  i >= 0 && tiles[i, tile.yIndex].GetTypeOfFood() == foodType; i--)
        {
            horizontalMatchList.Add(tiles[i, tile.yIndex]);
        }
        for (int i = tile.xIndex + 1; i < width && tiles[i, tile.yIndex].GetTypeOfFood() == foodType; i++)
        {
            horizontalMatchList.Add(tiles[i, tile.yIndex]);
        }

        List<Tile> verticalMatchList = new List<Tile>();
        verticalMatchList.Add(tile);

        for (int i = tile.yIndex - 1; i >= 0 && tiles[tile.xIndex, i].GetTypeOfFood() == foodType; i--)
        {
            verticalMatchList.Add(tiles[tile.xIndex, i]);
        }
        for (int i = tile.yIndex + 1; i < height && tiles[tile.xIndex, i].GetTypeOfFood() == foodType; i++)
        {
            verticalMatchList.Add(tiles[tile.xIndex, i]);
        }

        if (horizontalMatchList.Count < 3) horizontalMatchList = new List<Tile>();
        if (verticalMatchList.Count < 3) verticalMatchList = new List<Tile>();

        return horizontalMatchList.Union(verticalMatchList).ToList();
    }

    List<Tile> FindColumnMatches(int columnIndex)
    {
        List<Tile> tilesToDestroy = new List<Tile>();
        int foodType = tiles[columnIndex, 0].GetTypeOfFood();
        int foodInSequenceCount = 0;

        for(int j = 0; j < height; j++)
        {
            if(foodType == tiles[columnIndex, j].GetTypeOfFood())
            {
                foodInSequenceCount++;
                if(foodInSequenceCount == 3)
                {
                    tilesToDestroy.Add(tiles[columnIndex, j]);
                    tilesToDestroy.Add(tiles[columnIndex, j - 1]);
                    tilesToDestroy.Add(tiles[columnIndex, j - 2]);
                } 
                else if(foodInSequenceCount > 3)
                {
                    tilesToDestroy.Add(tiles[columnIndex, j]);
                }
            } 
            else
            {
                foodInSequenceCount = 1;
                foodType = tiles[columnIndex, j].GetTypeOfFood();
            }
        }

        return tilesToDestroy;
    }

    List<Tile> FindRowMatches(int rowIndex)
    {
        List<Tile> tilesToDestroy = new List<Tile>();
        int foodType = tiles[0, rowIndex].GetTypeOfFood();
        int foodInSequenceCount = 0;

        for (int i = 0; i < width; i++)
        {
            if (foodType == tiles[i, rowIndex].GetTypeOfFood())
            {
                foodInSequenceCount++;
                if (foodInSequenceCount == 3)
                {
                    tilesToDestroy.Add(tiles[i, rowIndex]);
                    tilesToDestroy.Add(tiles[i - 1, rowIndex]);
                    tilesToDestroy.Add(tiles[i - 2, rowIndex]);
                }
                else if (foodInSequenceCount > 3)
                {
                    tilesToDestroy.Add(tiles[i, rowIndex]);
                }
            }
            else
            {
                foodInSequenceCount = 1;
                foodType = tiles[i, rowIndex].GetTypeOfFood();
            }
        }

        return tilesToDestroy;
    }
}
