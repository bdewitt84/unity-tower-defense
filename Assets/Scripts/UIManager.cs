using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Brett DeWitt
// 
// Created: 05.03.2025
// 
// Description:
//    Simple event driven UI manager to update UI components

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerGoldText;
    [SerializeField] private TMP_Text gameClearText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text levelNumberText;

    [SerializeField] private int level;
    //
    // Subscribe to GameEvents events in start()
    //
    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += HandlePlayerHealthChanged;
        GameEvents.OnPlayerGoldChanged += HandlePlayerGoldChanged;
        GameEvents.OnStageClear += HandleStageClear;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnStageMove += HandleStageMove;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged -= HandlePlayerHealthChanged;
        GameEvents.OnPlayerGoldChanged -= HandlePlayerGoldChanged;
        GameEvents.OnStageClear -= HandleStageClear;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnStageMove -= HandleStageMove;
    }

    void Start()
    {
        levelNumberText.text = "Level " + level;
    }

    //
    // Event handlers
    //
    private void HandlePlayerHealthChanged(int updatedHealth)
    {
        playerHealthText.text = "Health: " + updatedHealth.ToString();
    }

    private void HandlePlayerGoldChanged(int updatedGold)
    {
        playerGoldText.text = "Gold: " + updatedGold.ToString();
        //GameEvents.StageMove();
    }

    private void HandleGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    private void HandleStageMove()
    {
        gameClearText.gameObject.SetActive(false);
        SceneManager.LoadScene("Level " + (level + 1));
    }

    private void HandleStageClear()
    {
        gameClearText.gameObject.SetActive(true);
    }

}
