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
        Wall
        //Goal
    }
    public int width = PersistentData.GridSize;
    public int height = PersistentData.GridSize;
    public float cellWidth = 1f;
    public float cellHeight = 1f;

    public GameObject gridObject;
    public GameObject goalPrefab;
    public GameObject agentPrefab;

    public Agent agent;
    public GoalObject goal;

    GridObject[,] grid;

    void Start()
    {
        width = PersistentData.GridSize;
        height = PersistentData.GridSize;
        MakeGrid();

        //grid[9,9].spaceType = spaceType.Goal;
        //grid[9,9].UpdateSpaceTypeDisplay();
        CreateGoal(PersistentData.GridSize-1, PersistentData.GridSize-1);
        grid[4,4].spaceType = spaceType.Wall;
        grid[4,4].UpdateSpaceTypeDisplay();

        CreateAgent(2, 3);
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

    void CreateAgent(int x, int y)
    {
        agent = Instantiate(agentPrefab, gameObject.transform).GetComponent<Agent>();
        agent.x = x;
        agent.y = y;
        agent.GridInit();
    }
    void CreateGoal(int x, int y)
    {
        goal = Instantiate(goalPrefab, gameObject.transform).GetComponent<GoalObject>();
        goal.x = x;
        goal.y = y;
        goal.GridInit();
    }
    void Update()
    {
        
    }
}
