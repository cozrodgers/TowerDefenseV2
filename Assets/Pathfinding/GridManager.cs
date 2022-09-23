using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridManager : MonoBehaviour
{
    [Tooltip("UnityGridSize should match UnityEditor snap settings")]
    [SerializeField] int unityGridSize = 10;
    [SerializeField] Vector2Int gridSize;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }
    public int UnityGridSize { get { return unityGridSize; } }
    void Awake()
    {
        //Create the grid
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }

        }

    }
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            //if the grid has the coords provided then set the isWalkable to false on the Node we provide
            grid[coordinates].isWalkable = false;
        }
    }
    public void ResetNodes() {
        foreach(KeyValuePair<Vector2Int,Node> entry in grid) {
            entry.Value.connectedTo  = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);
        Debug.Log(coordinates);
        return coordinates;
    }
    public Vector3 GetPositionFromCoordinates(Vector2Int coords)
    {
        Vector3 pos = new Vector3();
        pos.y = 0f;
        pos.x = coords.x * unityGridSize;
        pos.z = coords.y * unityGridSize;
        return pos;
    
       
    }
}
