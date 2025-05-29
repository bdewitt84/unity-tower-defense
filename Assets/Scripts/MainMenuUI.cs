using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Minsu Kim
//
// Created: 05.05.2025
//
// Description:
//   Handles the main menu UI actions such as starting the game and quitting the application.

public class MainMenuUI : MonoBehaviour
{
    // Loads the main game scene
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Level 1");
    }
}
