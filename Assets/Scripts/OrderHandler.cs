using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    private OrderLinker ol;
    private string[] currLikes = new string[2];
    private string[] currDislikes = new string[2];
    private int maxRandomNumber;


    // Start is called before the first frame update
    void Start()
    {
        ol = FindObjectOfType<OrderLinker>();
        maxRandomNumber = ol.TotalWeights();
        CreateNewOrder();
    }

    private void CreateNewOrder()
    {
        //print(ol.data.Length);
        int clueToTrySaving = WeighRandomNumber(maxRandomNumber);

        currLikes[0] = ol.GetClue(clueToTrySaving);

        clueToTrySaving = (int)Random.Range(0, ol.data.Length - 1);
        while(currLikes[1] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if (!ol.GetClue(clueToTrySaving).Equals(currLikes[0]))
            {
                currLikes[1] = ol.GetClue(clueToTrySaving);
            }
        }
        
        while(currDislikes[0] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1]))))
            {
                currDislikes[0] = ol.GetClue(clueToTrySaving);
            }
        }
        
        while (currDislikes[1] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1])) && (!ol.GetClue(clueToTrySaving).Equals(currDislikes[0]))))
            {
                currDislikes[1] = ol.GetClue(clueToTrySaving);
            }
        }

        PrintOrder();
    }

    //Used for debugging
    public void PrintOrder()
    {
        print("Likes:\n" + currLikes[0] + "\n" + currLikes[1] + "\nDislikes:\n" + currDislikes[0] + "\n" + currDislikes[1]);

        print("Objects for Like #1:" + ol.GetObjectsForClueString(currLikes[0]));

    }

    public int WeighRandomNumber(int max)
    {
        int unweighted = Random.Range(0, max);
        //print("Unweighted:" + unweighted);

        int[,] weightedValues = ol.WeightedArray();

        for(int i=0; i<ol.data.Length; i++)
        {
            if(weightedValues[i,0]<= unweighted && unweighted < weightedValues[i,1])
            {
                return i;
            }

        }

        return 0;
    }

    public int CheckOrder(Constants.Objects[] choices)
    {
        int score = 0;
        Constants.Objects[] checkMe;
        for(int i=0; i< choices.Length; i++)
        {
            for(int k=0; k<2; k++)
            {
                checkMe = ol.GetObjectsForClue(currLikes[k]);
                for (int j = 0; j < checkMe.Length; j++)
                {
                    if (checkMe[j] == choices[i])
                    {
                        score++;
                    }
                }
            }
            for (int k = 0; k < 2; k++)
            {
                checkMe = ol.GetObjectsForClue(currDislikes[k]);
                for (int j = 0; j < checkMe.Length; j++)
                {
                    if (checkMe[j] == choices[i])
                    {
                        score--;
                    }
                }
            }
        }
        print(score);

        return score;
        
    }

}
