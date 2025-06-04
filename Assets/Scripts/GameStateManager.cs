// ./Assets/Scripts/Managers/GameStateManager.cs

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
    [SerializeField] private float timer = 0.0f;
    private bool isGameOver = false;

    private void Start()
    {
        playerHealth = playerHealthStart;
        GameEvents.OnPlayerHealthChanged(playerHealth);
        playerGold = playerGoldStart;
        GameEvents.OnPlayerGoldChanged(playerGold);
    }
    private void Update()
    {
        if (isGameOver) return;

        if (playerHealth <= 0)
        {
            isGameOver = true;
            GameEvents.GameOver();
            return;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                Debug.Log("Timer has finished");
                GameEvents.StageMove();
            }
        }
    }
    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess += HandleTowerPlacementSuccess;
        GameEvents.OnStageClear += HandleStageClear;
        GameEvents.OnGameOver += HandleGameOver; 
    }


    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess -= HandleTowerPlacementSuccess;
        GameEvents.OnStageClear -= HandleStageClear;
        GameEvents.OnGameOver -= HandleGameOver;

    }

    private void HandleEnemyReachedGoal(EnemyController enemy, float damage)
    {

        if (isGameOver) return;

        playerHealth -= (int)damage;
        GameEvents.PlayerHealthChanged(playerHealth);

        if (playerHealth <= 0 && !isGameOver)
        {
            isGameOver = true;
            GameEvents.GameOver(); 
        }
    }

    private void HandleGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        Debug.Log("Game Over triggered");
    }

    private void HandleEnemyKilled(EnemyController enemy, float reward)
    {
        playerGold += (int)reward;
        GameEvents.PlayerGoldChanged(playerGold);
    }

    private void HandleTowerPlacementSuccess(GameObject towerInstance)
    {
        TowerController towerController = towerInstance.GetComponent<TowerController>();
        int cost = towerController.GetCost();
        playerGold -= cost;
        GameEvents.PlayerGoldChanged(playerGold);
    }

    public int GetPlayerGold()
    {
        return playerGold;
    }

    public void HandleStageClear()
    {
        timer = 5.0f;
        Debug.Log("Set timer to 5 sec");
    }
}
