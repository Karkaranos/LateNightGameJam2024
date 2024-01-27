/*****************************************************************************
// File Name :         TitleScreenController.cs
// Author :            Cade R. Naylor
// Creation Date :     January 27, 2024
//
// Brief Description : Handles button functionality for the title screen 

*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen;
    [SerializeField]
    private GameObject creditScreen;

    /// <summary>
    /// Opens the Game's credits and closes the title screen
    /// </summary>
    public void OpenCredits()
    {
        creditScreen.SetActive(true);
        titleScreen.SetActive(false);
    }

    /// <summary>
    /// Closes the game's credits and opens the title scene
    /// </summary>
    public void CloseCredits()
    {
        creditScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Starts the tutorial
    /// </summary>
    public void StartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
