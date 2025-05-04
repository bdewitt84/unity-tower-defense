using System;
using UnityEngine;

// Author: Brett DeWitt
//
// Created: 05/03/2025
//
// Description:
//   Handles state of the game including player lives and gold. State is
//   managed via subscribing to events defined in the GameEvents class.
//   Responsible for handling win conditions and lose conditions.

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerHealthStart = 100;

    private void Start()
    {
        playerHealth = playerHealthStart;
        GameEvents.OnPlayerHealthChanged(playerHealth);
    }

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
        playerHealth -= 5;
        GameEvents.PlayerHealthChanged(playerHealth);
    }

    private void HandleEnemyKilled(EnemyController enemy)
    {
        playerGold += 5;
    }

}
