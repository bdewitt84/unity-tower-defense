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

    override public bool AtCurrentWaypoint()
    {
        if (currentWaypoint == null)
        {
            Debug.LogWarning("[EnemyController] Current waypoint is null.");
            return false;
        }
        else
        {
            float margin = 0.1f;
            float distanceFromWaypoint = Vector3.Distance(parent.transform.position,
                                                          currentWaypoint.position);
            return (distanceFromWaypoint < margin);
        }
    }


    // Returns true if the last visited waypoint was the final waypoint in the
    // waypoints list
    override public bool ReachedGoal()
    {
        return (waypointIndex >= lane.childCount);
    }

    // Sets the current waypoint to the next in the waypoints list, and
    // unpacks its x and z coordinates into a destination vector for the
    // enemy to move toward. The y coordinate is kept the same to ensure
    // that the enemy's height does not change.
    override public void SetNextWaypoint()
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

    override public bool CanSetNextWaypoint(out string reason)
    {
        reason = "";

        if (waypoints == null)
        {
            reason = "Cannot set next waypoint. Waypoints is null.";
            return false;
        }

        if (waypoints.Count == 0)
        {
            reason = "Cannot set next waypoint. Waypoints list is empty.";
            return false;
        }

        if (waypointIndex < 0 || waypointIndex >= waypoints.Count)
        {
            reason = "Cannot set next waypoint. Index is out of range.";
            return false;
        }

        return true;
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

    public override Vector3 GetCurrentWaypointPosition()
    {
        return currentWaypoint.position;
    }
}
