using System;
using System.Collections.Generic;
using UnityEngine;

public class LanePathFinder : PathfindingComponent
{
    private Transform lane;
    private List<Transform> waypoints = new();
    private int waypointIndex = 0;
    private Transform currentWaypoint;


    public LanePathFinder(EnemyController parent, Transform lane) : base(parent)
    {
        SetLane(lane);
        SetNextWaypoint();
    }

    override public void Update()
    {
        if (AtCurrentWaypoint())
        {
            if (HasMoreWaypoints())
            {
                SetNextWaypoint();
            }
        }
    }

     public bool AtCurrentWaypoint()
    {

        float margin = 0.1f;
        float distanceFromWaypoint = Vector3.Distance(parent.transform.position,
                                                        currentWaypoint.position);
        return (distanceFromWaypoint < margin);

    }

    // Returns true if the last visited waypoint was the final waypoint in the
    // waypoints list
    override public bool HasReachedGoal()
    {
        return !HasMoreWaypoints() && AtCurrentWaypoint();
    }

    // Sets the current waypoint to the next in the waypoints list, and
    // unpacks its x and z coordinates into a destination vector for the
    // enemy to move toward. The y coordinate is kept the same to ensure
    // that the enemy's height does not change.
    public void SetNextWaypoint()
    {
        // Get next from list
        currentWaypoint = waypoints[waypointIndex];

        // Normalize height
        currentWaypoint.position = new Vector3(currentWaypoint.transform.position.x,
                                  parent.transform.position.y,
                                  currentWaypoint.transform.position.z);

        Debug.Log($"CurrentWaypointPosition: {currentWaypoint.position}");

        // Advance index
        waypointIndex += 1;
    }

    public bool HasMoreWaypoints()
    {
        return waypointIndex < waypoints.Count;
    }

    // Adds every child of lane to list of waypoints for enemy to follow.
    // The lane must first be set with SetLane()
    public void GetWaypointsFromLane()
    {
        if (lane == null)
        {
            Debug.Log("GetWaypointsFromLane must have non-null lane. Use" +
                "SetLane() to assign a lane to Enemy.");
        }
        else
        {
            waypoints.Clear();
            foreach (Transform waypoint in lane)
            {
                waypoints.Add(waypoint);
            }
        }
    }
    public void SetLane(Transform lane)
    {
        if (lane == null)
        {
            throw new ArgumentNullException("Lane must not be null");
        }

        this.lane = lane;
        GetWaypointsFromLane();

        if (waypoints.Count <= 0)
        {
            Debug.LogError("[EnemyController] Lane has no waypoints. Destroying self.");
        }
    }

    override public Vector3 GetCurrentWaypointPosition()
    {
        return currentWaypoint.position;
    }
}
