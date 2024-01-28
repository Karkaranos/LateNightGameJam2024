using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text OrderTimer;
    private int timeLeft;
    private int startTime;
    private Coroutine timerCoroutine;
    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        startTime = (int)(gc.DayTime * 60);
        //OrderTimer.text = timeLeft.ToString();
        //timerCoroutine = StartCoroutine(Clock());
    }


    private IEnumerator Clock()
    {
        while(timeLeft > -1)
        {
            if(timeLeft % 60 < 10)
            {
                OrderTimer.text = timeLeft / 60 + ":0" + timeLeft%60;
            }
            else
            {

                OrderTimer.text = timeLeft / 60 + ":" + timeLeft % 60;
            }
            timeLeft -= 1;
            yield return new WaitForSeconds(1);
        }

        gc.RoundEndFunc();
    }

    public void StartTimer()
    {
        timeLeft = startTime;
        timerCoroutine = StartCoroutine(Clock());
    }
}
