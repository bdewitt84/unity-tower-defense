using System;
using UnityEngine;

public class DistanceMapPathFinder : PathfindingComponent
{
    EnemyController parent;
    DistanceMap distanceMap;
    GridCoordinate currentCell;
    Vector3 currentWaypoint;

    public DistanceMapPathFinder(EnemyController parent)
    {
        this.parent = parent;
    }
    public void Update()
    {
        if (AtCurrentWaypoint() && HasMoreWaypoints())
        {
            SetNextWaypoint();
        }
    }

    private bool HasMoreWaypoints()
    {
        throw new NotImplementedException();
    }

    private bool AtCurrentWaypoint()
    {
        return false;
    }

    private void SetNextWaypoint()
    {
        int minDistance = int.MaxValue;
        GridCoordinate nextCell;

        foreach (var neighbor in distanceMap.GetNeighbors(currentCell))
        {
            int neighborDistance = distanceMap.GetDistance(neighbor);
            if (neighborDistance < minDistance)
            {
                minDistance = neighborDistance;
                nextCell = neighbor;
            }
        }
        // convert grid coordinate to vector 3 waypoint
    }

    public Vector3 GetCurrentWaypointPosition()
    {
        throw new System.NotImplementedException();
    }

    public bool HasReachedGoal()
    {
        throw new System.NotImplementedException();
    }
}
