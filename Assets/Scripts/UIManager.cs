using System;
using TMPro;
using UnityEngine;

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


    //
    // Subscribe to GameEvents events in start()
    //
    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += HandlePlayerHealthChanged;
        GameEvents.OnPlayerGoldChanged += HandlePlayerGoldChanged;
        GameEvents.OnGameClear += HandleGameClear;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHealthChanged -= HandlePlayerHealthChanged;
        GameEvents.OnPlayerGoldChanged -= HandlePlayerGoldChanged;
        GameEvents.OnGameClear -= HandleGameClear;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    void Start()
    {
        
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
    }

    private void HandleGameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    private void HandleGameClear()
    {
        gameClearText.gameObject.SetActive(true);
    }

}
