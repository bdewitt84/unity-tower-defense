using UnityEngine;

public class AudioManager
{

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
    }


    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
    }

    private void HandleEnemyReachedGoal(EnemyController enemy)
    {
        // play enemy reached goal sound
    }

    private void HandleEnemyKilled(EnemyController enemy)
    {
        // play enemy killed sound
    }

}