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

    //
    // Subscribe to GameEvents events in start()
    //
    void Start()
    {
        GameEvents.OnPlayerHealthChanged += HandlePlayerHealthChanged;
        GameEvents.OnPlayerGoldChanged += HandlePlayerGoldChanged;
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
}
