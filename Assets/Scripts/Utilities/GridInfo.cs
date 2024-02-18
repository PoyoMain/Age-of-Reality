using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GridSpot
{
    private readonly GameObject spot;

    public Vector2Int Position
    {
        get
        {
            return new((int)spot.transform.localPosition.x, (int)spot.transform.localPosition.z);
        }
    }

    public Vector3 WorldPosition
    {
        get
        {
            return spot.transform.position;
        }
    }

    public bool filled;

    public GridSpot(Transform parent, Vector3 pos, float scale = 1)
    {
        spot = new GameObject("(" + (pos.x / scale) + "," + (pos.y / scale) + ")");
        spot.transform.parent = parent.transform;
        spot.transform.localPosition = pos;
    }

    public int gCost = 10;
    public int hCost;
    public int fCost;

    public List<GridSpot> AdjacentSpots
    {
        get
        {
            return GetAdjacentSpots();
        }
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    private List<GridSpot> GetAdjacentSpots()
    {
        List<GridSpot> adjList = new ();
        GridSpot spot = GridInfo.FindGridSpot(Position.x + 1, Position.y);
        if (spot != null ) 
        {
            adjList.Add(spot);
        }

        spot = GridInfo.FindGridSpot(Position.x, Position.y - 1);
        if (spot != null)
        {
            adjList.Add(spot);
        }

        spot = GridInfo.FindGridSpot(Position.x - 1, Position.y);
        if (spot != null)
        {
            adjList.Add(spot);
        }

        spot = GridInfo.FindGridSpot(Position.x, Position.y + 1);
        if (spot != null)
        {
            adjList.Add(spot);
        }

        return adjList;
    }
}

public class GridInfo : MonoBehaviour
{
    public static GridSpot[,] grid;

    public int rows = 5;
    public int cols = 5;
    public float scaleMult = 1;

    public static Vector2 GridMidPoint
    {
        get;
        private set;
    }

    public static Vector3 GridWorldMidPoint
    { 
        get; 
        private set; 
    }

    // Start is called before the first frame update
    void Awake()
    {
        MakeGrid();
    }

    void MakeGrid()
    {
        grid = new GridSpot[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 pos = new((i * scaleMult), (j * scaleMult), transform.position.z);
                grid[i, j] = new(transform, pos, scaleMult);
            }
        }

        GridMidPoint = GetMiddleOfGrid();
        GridWorldMidPoint = GetWorldMiddleOfGrid();
    }

    public static GridSpot FindGridSpot(int x, int z)
    {
        if (grid == null)
        {
            return null;
        }

        if (x < 0 || x >= grid.GetLength(0) || z < 0 || z >= grid.GetLength(1))
        {
            return null;
        }

        return grid[x, z];
    }

    public static GridSpot FindGridSpot(Vector2Int pos)
    {
        if (grid == null)
        {
            return null;
        }

        if (pos.x < 0 || pos.x >= grid.GetLength(0) || pos.y < 0 || pos.y >= grid.GetLength(1))
        {
            return null;
        }

        return grid[pos.x, pos.y];
    }

    private Vector2 GetMiddleOfGrid()
    {
        float x = grid[0, rows - 1].Position.x - grid[0,0].Position.x;
        float y = grid[0, cols - 1].Position.y - grid[0,0].Position.y;

        return new(x, y);
    }

    private Vector3 GetWorldMiddleOfGrid()
    {
        float x1 = grid[cols - 1, 0].WorldPosition.x;
        float x2 = grid[0, 0].WorldPosition.x;
        float y1 = grid[0, rows - 1].WorldPosition.y;
        float y2 = grid[0, 0].WorldPosition.y;

        Vector2 midpoint = new((x1 + x2) / 2, (y1 + y2) / 2);

        return new(midpoint.x, midpoint.y, transform.position.z);
    }
}
