using UnityEngine;

public interface PathfindingComponent
{
    abstract public void Update();

    abstract public bool HasReachedGoal();

    abstract public Vector3 GetCurrentWaypointPosition();
}
