using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen;
    [SerializeField]
    private GameObject creditScreen;

    public void OpenCredits()
    {
        creditScreen.SetActive(true);
        titleScreen.SetActive(false);
    }

    public void CloseCredits()
    {
        creditScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
