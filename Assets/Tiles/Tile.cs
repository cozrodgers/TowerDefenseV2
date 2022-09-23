using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    Vector2Int coords = new Vector2Int();
    GridManager gridManager;
    Pathfinder pathfinder;
    [SerializeField] Tower towerPf;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }
    void Awake()
    {
        pathfinder = FindObjectOfType<Pathfinder>();
        gridManager = FindObjectOfType<GridManager>();
    }
    void Start()
    {
        if (gridManager != null)
        {
            coords = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable)
            {
                gridManager.BlockNode(coords);
            }

        }

    }
    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (gridManager.GetNode(coords).isWalkable && !pathfinder.WillBlockPath(coords))
        {
            bool isPlaced = towerPf.CreateTower(towerPf, transform.position);
            isPlaceable = !isPlaced;
            gridManager.BlockNode(coords);
        }
    }

    void IPointerExitHandler.OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Debug.Log("Exit");
    }




}
