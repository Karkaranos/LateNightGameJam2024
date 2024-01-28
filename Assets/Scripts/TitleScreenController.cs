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
    private AudioManager am;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        if (PlayerPrefs.HasKey("HighScore") == false)
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }

    /// <summary>
    /// Opens the Game's credits and closes the title screen
    /// </summary>
    public void OpenCredits()
    {
        creditScreen.SetActive(true);
        titleScreen.SetActive(false);
        if(am!=null)
        {
            am.PlayClick();
        }
    }

    /// <summary>
    /// Closes the game's credits and opens the title scene
    /// </summary>
    public void CloseCredits()
    {
        creditScreen.SetActive(false);
        titleScreen.SetActive(true);
        if (am != null)
        {
            am.PlayClick();
        }
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
        if (am != null)
        {
            am.PlayClick();
        }
    }

    /// <summary>
    /// Starts the tutorial
    /// </summary>
    public void StartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
        if (am != null)
        {
            am.PlayClick();
            am.PlayTutorialMusic();
        }

    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        if (am != null)
        {
            am.PlayClick();
        }
    }
}
