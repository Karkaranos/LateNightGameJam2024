using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text OrderTimer;
    public int timeLeft;
    public int startTime;
    private Coroutine timerCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        OrderTimer.text = timeLeft.ToString();
        timerCoroutine = StartCoroutine(Clock());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Clock()
    {
        while(gameObject != null)
        {
            yield return new WaitForSeconds(1);
            timeLeft -= 1;
            OrderTimer.text = timeLeft.ToString();
            if(timeLeft <= 0)
            {
                timeLeft = startTime;
            }
        }
    }

    public void RestartTimer()
    {
        StopCoroutine(timerCoroutine);
        timeLeft = startTime;
        timerCoroutine = StartCoroutine(Clock());
    }
}
