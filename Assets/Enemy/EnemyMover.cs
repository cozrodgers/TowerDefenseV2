using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{

    [SerializeField][Range(0f, 5f)] float speed = 1f;
    List<Node> path = new List<Node>();
    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }


    void FindPath()
    {

        path.Clear();
        path = pathfinder.GetNewPath();
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath()
    {
        // Loop over the coords
        for (int i = 0; i < path.Count; i++)
        {
            
                // set transform.position to position of the wp
                Vector3 startPos = transform.position;
                Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);
                float travelPercent = 0;
                transform.LookAt(endPos);
                while (travelPercent < 1f)
                {
                    travelPercent += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                    yield return new WaitForEndOfFrame();
                }
            
        }
            FinishPath();
    }

        void ReturnToStart()
        {
            transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoords);
        }
    }
