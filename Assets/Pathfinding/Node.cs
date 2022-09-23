using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node 
{
    //Pure C# Class
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;
    //Constructor
    public Node(Vector2Int coordinates, bool isWalkable) {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }




}
