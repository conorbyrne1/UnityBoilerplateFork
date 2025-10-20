using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update

    public enum spaceType
    {
        Empty,
        Rough,
        Wall,
        Goal
    }
    public int width = 10;
    public int height = 10;
    public float cellWidth = 1f;
    public float cellHeight = 1f;

    public GameObject gridObject;

    GridObject[,] grid;

    void Start()
    {
        MakeGrid();

    }


    void MakeGrid()
    {
        grid = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = Instantiate(gridObject, gameObject.transform).GetComponent<GridObject>();
                grid[x, y].x = x;
                grid[x, y].y = y;
                grid[x, y].spaceType = spaceType.Empty;
                grid[x, y].GridInit();
            }
        }

    }

    public spaceType GetCellState(int x, int y)
    {
        return grid[x, y].spaceType;
    }

    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x * cellWidth - ((float)width /2), 0, y * cellHeight - ((float)height / 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
