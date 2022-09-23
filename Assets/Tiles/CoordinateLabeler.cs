using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f,0f,0.5f);
    [SerializeField] Color blockedColor = Color.gray;
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        DisplayCoordinates();

    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            //do something
            DisplayCoordinates();
            UpdateObjectName();
        }
        ToggleLabels();
        SetLabelColor();
        
    }
    void SetLabelColor()
    {
        //Look for grid manager
        if(gridManager == null) {
            return;
        } 
        

        //if there's a grid manager, set the colors
        //Store a reference to the Node we are looking at
        Node node = gridManager.GetNode(coordinates);
        if(node == null) return;
         if(!node.isWalkable){
            label.color = blockedColor;
         } else if(node.isPath) {
            label.color = pathColor;
         } else if(node.isExplored) {
            label.color = exploredColor;
         } else {
            label.color = defaultColor;
         }
    }

    void DisplayCoordinates()
    {
        if(gridManager  == null ) {
            return;
        }
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }
}
