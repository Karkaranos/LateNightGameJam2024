/*****************************************************************************
// File Name :         Dream.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Handles collisions with the dream "pool". Stores objects
                        currently on it and calls checking the match to the order.

*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    //References to other scripts
    private OrderHandler oh;
    private GameController gc;

    //Required for validation
    private int overlappingIngredients;
    int result;
    private Constants.Objects[] checkMe = new Constants.Objects[3];
    private List<GameObject> currentObjects = new List<GameObject>();

    /// <summary>
    /// Start is called on the first frame update. It gets references to other scripts.                                
    /// </summary>
    private void Start()
    {
        oh = FindObjectOfType<OrderHandler>();
        gc = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!currentObjects.Contains(collision.gameObject) && overlappingIngredients < 3)
        {
            currentObjects.Add(collision.gameObject);
            overlappingIngredients++;

            if (overlappingIngredients == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    checkMe[i] = currentObjects[i].GetComponent<ConstantStorage>().itemName;
                }
                ReleaseDream();
                gc.HandleResults(oh.CheckOrder(checkMe));
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentObjects.Contains(collision.gameObject))
        {
            currentObjects.Remove(collision.gameObject);
            overlappingIngredients--;
        }
    }

    private void ReleaseDream()
    {
        GameObject[] deleteMe = new GameObject[3];
        for(int i=0; i<3; i++)
        {
            deleteMe[i] = currentObjects[i];
            gc.shelvesVis[currentObjects[i].GetComponent<ConstantStorage>().index] = null;
        }
        currentObjects.Clear();
        overlappingIngredients = 0;

        for(int i=0; i<3; i++)
        {
            Destroy(deleteMe[i]);
        }
    }
}
