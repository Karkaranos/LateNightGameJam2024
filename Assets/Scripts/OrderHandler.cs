using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    private OrderLinker ol;
    private string[] currLikes = new string[2];
    private string[] currDislikes = new string[2];


    // Start is called before the first frame update
    void Start()
    {
        ol = FindObjectOfType<OrderLinker>();
        CreateNewOrder();
    }

    private void CreateNewOrder()
    {
        int clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1); ;

        currLikes[0] = ol.GetClue(clueToTrySaving);

        clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1);
        while(currLikes[1] == null)
        {
            clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1);
            if (!ol.GetClue(clueToTrySaving).Equals(currLikes[0]))
            {
                currLikes[1] = ol.GetClue(clueToTrySaving);
            }
        }

        while(currDislikes[0] == null)
        {
            clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1]))))
            {
                currDislikes[0] = ol.GetClue(clueToTrySaving);
            }
        }

        while(currDislikes[1] == null)
        {
            clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1])) && (!ol.GetClue(clueToTrySaving).Equals(currDislikes[0]))))
            {
                currDislikes[1] = ol.GetClue(clueToTrySaving);
            }
        }

        PrintOrder();
    }

    public void PrintOrder()
    {
        print("Likes:\n" + currLikes[0] + "\n" + currLikes[1] + "\nDislikes:\n" + currDislikes[0] + "\n" + currDislikes[1]);

        print("Objects for Like #1:" + ol.GetObjectsForClue(currLikes[0]));

    }

}
