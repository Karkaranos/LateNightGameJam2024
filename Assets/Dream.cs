using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    private int overlappingIngredients;
    private OrderHandler oh;
    private GameController gc;
    int result;
    Constants.Objects[] checkMe = new Constants.Objects[3];

    public List<GameObject> currentObjects = new List<GameObject>();

    private void Start()
    {
        oh = FindObjectOfType<OrderHandler>();
        gc = FindObjectOfType<GameController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!currentObjects.Contains(collision.gameObject) && overlappingIngredients<3)
        {
            currentObjects.Add(collision.gameObject);
            overlappingIngredients++;

            if(overlappingIngredients == 3)
            {
                for(int i=0; i<3; i++)
                {
                    checkMe[i] = currentObjects[i].GetComponent<ConstantStorage>().itemName;
                }
                ReleaseDream();
                gc.HandleResults(oh.CheckOrder(checkMe));
            }
        }
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(currentObjects.Contains(collision.gameObject))
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
