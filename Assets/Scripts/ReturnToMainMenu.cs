// Scripts/ReturnToMainMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public void OnClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}