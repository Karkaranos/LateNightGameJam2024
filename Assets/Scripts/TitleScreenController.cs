/*****************************************************************************
// File Name :         TitleScreenController.cs
// Author :            Cade R. Naylor
// Creation Date :     January 27, 2024
//
// Brief Description : Handles button functionality for the title screen 

*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen;
    [SerializeField]
    private GameObject creditScreen;
    private AudioManager am;
    [SerializeField]
    private GameObject credits;
    Coroutine stopMe;

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
        //credits.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        creditScreen.SetActive(true);
        titleScreen.SetActive(false);
        if(am!=null)
        {
            am.PlayClick();
        }
        stopMe = StartCoroutine(CreditScroll());
    }

    /// <summary>
    /// Scrolls through the credits at a set rate
    /// </summary>
    /// <returns>Time between each position incrmement</returns>
    IEnumerator CreditScroll()
    {
        credits.transform.position = new Vector2(Screen.width / 2, Screen.height/8);
        for (int i = 0; i < 1500; i += 1)
        {
            Vector2 creditPos = credits.transform.position;
            creditPos.y += Screen.height / 500f;
            credits.transform.position = creditPos;
            yield return new WaitForSeconds(.02f);
        }
        stopMe = StartCoroutine(CreditScroll());

    }


    /// <summary>
    /// Closes the game's credits and opens the title scene
    /// </summary>
    public void CloseCredits()
    {
        StopCoroutine(stopMe);
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
