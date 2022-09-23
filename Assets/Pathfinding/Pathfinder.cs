using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoords;
    public Vector2Int StartCoords
    {
        get
        {
            return startCoords;
        }
    }
    [SerializeField] Vector2Int destinationCoords;
    public Vector2Int DestinationCoords { get { return destinationCoords; } }
    Node startNode;
    Node destinationNode;
    [SerializeField] Node currentSearchNode;

    Dictionary<Vector2Int, Node> reachedDictionary = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Queue<Node> frontier = new Queue<Node>();
    [SerializeField] Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoords];
            destinationNode = grid[destinationCoords];
            //Set start and destination nodes as walkable 
        }
    }
    void Start()
    {
        GetNewPath();

    }
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoords);
    }
    public List<Node> GetNewPath(Vector2Int coords)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coords);
        return BuildPath();
    }
    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;
            //check if the coords are in the grid
            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
            // loop over the neighbors we found in the grid and add them to the frontier
            foreach (Node neighbor in neighbors)
            {
                //check if already reached and whether the node is walkable or not
                if (!reachedDictionary.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
                {
                    neighbor.connectedTo = currentSearchNode;
                    //add the neighbor to frontier and reached dict
                    reachedDictionary.Add(neighbor.coordinates, neighbor);
                    frontier.Enqueue(neighbor);

                }
            }
        }
    }
    void BreadthFirstSearch(Vector2Int coords)
    {

        startNode.isWalkable = true;
        destinationNode.isWalkable = true;
        //clear frontier and reached;
        frontier.Clear();
        reachedDictionary.Clear();

        bool isRunning = true;

        frontier.Enqueue(grid[coords]);
        reachedDictionary.Add(startCoords, grid[coords]);

        while (frontier.Count > 0 && isRunning)
        {
            //set the current search node to the next node in the frontier lis
            currentSearchNode = frontier.Dequeue(); // set the current node to explored
            currentSearchNode.isExplored = true;
            // add all the neighbors
            ExploreNeighbors();
            if (currentSearchNode.coordinates == destinationCoords)
            {
                //stop running when we find our destination
                isRunning = false;
            }

        }
    }
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;
        //go back through the nodes and add them to the path
        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
            Debug.Log(path.Count);

        }
        path.Reverse();
        return path;
    }


    public bool WillBlockPath(Vector2Int coords)
    {
        //check the coords we pass in are valid
        if (grid.ContainsKey(coords))
        {
            bool prevState = grid[coords].isWalkable;
            grid[coords].isWalkable = false;
            //generate a new path;
            List<Node> newPath = GetNewPath();
            //set the state back
            grid[coords].isWalkable = prevState;


            //path will be blocked so generate new path
            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }

            //able to find a valid path
        }
        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);

    }
}