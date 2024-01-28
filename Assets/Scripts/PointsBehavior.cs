/*****************************************************************************
// File Name :         PointsBehavior.cs
// Author :            Cade R. Naylor
// Creation Date :     January 28, 2024
//
// Brief Description : Makes spawned points fade over time

*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PointsBehavior : MonoBehaviour
{
    private Vector2 pos;
    private GameObject self;

    [SerializeField]
    private float existenceTime;
    [SerializeField]
    private int heightAdjustment;

    private int amount;

    private TMP_Text textSays;



    private Color currColor;

    public int Amount {set => amount = value; }

    // Start is called before the first frame update
    void Start()
    {
        textSays = GetComponent<TMP_Text>();
        pos = transform.position;
       textSays.text = "+ $" + amount;
        currColor = GetComponent<TextMeshProUGUI>().faceColor;

        StartCoroutine(Dissapear());
    }

    IEnumerator Dissapear()
    {
        float timeLeft = existenceTime;
        float ratio;
        while(timeLeft > 0)
        {
            ratio = timeLeft / existenceTime;
            GetComponent<TextMeshProUGUI>().faceColor = new Color(currColor.r, currColor.g, currColor.b, ratio);
            timeLeft -= Time.deltaTime;
            pos.y += (Screen.height / heightAdjustment);
            transform.position = pos;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


}
