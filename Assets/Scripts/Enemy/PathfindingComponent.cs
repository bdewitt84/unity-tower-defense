using UnityEngine;

public abstract class PathfindingComponent
{
    protected EnemyController parent;
    public Vector3 destination;

    public PathfindingComponent(EnemyController parent)
    {
        this.parent = parent;
    }
    abstract public void Update();

    // abstract public bool AtCurrentWaypoint();

    abstract public bool HasReachedGoal();

    // abstract public bool HasMoreWaypoints();

    // abstract public void SetNextWaypoint();

    abstract public Vector3 GetCurrentWaypointPosition();
}
