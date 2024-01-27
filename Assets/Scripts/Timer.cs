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

    // Start is called before the first frame update
    void Start()
    {
        OrderTimer.text = timeLeft.ToString();
        StartCoroutine(Clock());
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
}
