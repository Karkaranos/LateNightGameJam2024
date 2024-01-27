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

    /// <summary>
    /// Stores new objects that collide with it. Checks dream if min met. 
    /// </summary>
    /// <param name="collision">The object collided with</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the new collision is not in the object list and there are less than 3 ingredients
        if (!currentObjects.Contains(collision.gameObject) && overlappingIngredients < 3)
        {
            currentObjects.Add(collision.gameObject);
            overlappingIngredients++;

            //If the required amount of ingredients per dream is met, check it
            if (overlappingIngredients == 3)
            {
                //Store them in an array that can be checked and release the dream
                for (int i = 0; i < 3; i++)
                {
                    checkMe[i] = currentObjects[i].GetComponent<ConstantStorage>().itemName;
                }
                ReleaseDream();
                gc.HandleResults(oh.CheckOrder(checkMe));
            }
        }
    }

    /// <summary>
    /// Removes an object upon leaving collision
    /// </summary>
    /// <param name="collision">The object collided with</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the collision object had been overlapping, remove it from the list
        if (currentObjects.Contains(collision.gameObject))
        {
            currentObjects.Remove(collision.gameObject);
            overlappingIngredients--;
        }
    }

    /// <summary>
    /// Remove the objects from the game, both visually and from the arrays. Resets variables. 
    /// </summary>
    private void ReleaseDream()
    {
        GameObject[] deleteMe = new GameObject[3];
        //Null the array references out
        for(int i=0; i<3; i++)
        {
            deleteMe[i] = currentObjects[i];
            gc.ShelvesVis[currentObjects[i].GetComponent<ConstantStorage>().index] = null;
        }

        currentObjects.Clear();
        overlappingIngredients = 0;

        //Delete the objects
        for(int i=0; i<3; i++)
        {
            Destroy(deleteMe[i]);
        }
    }
}
