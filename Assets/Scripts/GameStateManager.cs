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
    [SerializeField] private int playerGold;
    [SerializeField] private int playerHealthStart = 100;
    [SerializeField] private int playerGoldStart = 50;

    private void Start()
    {
        playerHealth = playerHealthStart;
        GameEvents.OnPlayerHealthChanged(playerHealth);
        playerGold = playerGoldStart;
        GameEvents.OnPlayerGoldChanged(playerGold);
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess += HandleTowerPlacementSuccess;
    }


    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess -= HandleTowerPlacementSuccess;

    }

    private void HandleEnemyReachedGoal(EnemyController enemy)
    {
        playerHealth -= 5;
        GameEvents.PlayerHealthChanged(playerHealth);
        if (playerHealth <= 0)
        {
            GameEvents.GameOver();
        }
    }

    private void HandleEnemyKilled(EnemyController enemy)
    {
        playerGold += 5;
        GameEvents.PlayerGoldChanged(playerGold);
    }

    private void HandleTowerPlacementSuccess(TowerController tower)
    {
        int cost = tower.GetCost();
        if (playerGold >= cost)
        {
            playerGold -= cost;
            GameEvents.PlayerGoldChanged(playerGold);
        }
        else
        {
            Destroy(tower.gameObject);
        }
    }

    public int GetPlayerGold()
    {
        return playerGold;
    }
}
