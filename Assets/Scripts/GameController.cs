using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] shelvesVis = new GameObject[9];
    Objects[,] shelvesCont = new Objects[3, 3];
    ObjectHandler oh;

    private void Start()
    {
        oh = FindObjectOfType<ObjectHandler>();
        RefillAllShelves();
    }
    private void RefillAllShelves()
    {
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                shelvesCont[i, j] = oh.items[oh.WeighRandomNumber()];
                shelvesVis[i+j].GetComponent<SpriteRenderer>().sprite = shelvesCont[i, j].visual;
            }
        }

    }

}
