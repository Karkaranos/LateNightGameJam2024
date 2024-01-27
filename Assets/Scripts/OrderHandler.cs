/*****************************************************************************
// File Name :         OrderHandler.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Creates orders following a weighted random system. No clue may 
                        be used twice per order. Checks if the order is correct.

*****************************************************************************/
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    private OrderLinker ol;
    private string[] currLikes = new string[2];
    private string[] currDislikes = new string[2];
    private int maxRandomNumber;


    /// <summary>
    /// Start is called before the first frame update. It gets references to other scripts and creates an order. 
    /// </summary>
    void Start()
    {
        ol = FindObjectOfType<OrderLinker>();
        maxRandomNumber = ol.TotalWeights();
        CreateNewOrder();
    }

    /// <summary>
    /// Creates a new order using weighted random numbers
    /// </summary>
    private void CreateNewOrder()
    {
        //Create the first clue
        int clueToTrySaving = WeighRandomNumber(maxRandomNumber);
        currLikes[0] = ol.GetClue(clueToTrySaving);

        //Get a second clue different than the first
        while(currLikes[1] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if (!ol.GetClue(clueToTrySaving).Equals(currLikes[0]))
            {
                currLikes[1] = ol.GetClue(clueToTrySaving);
            }
        }
        
        //Get a third clue different than the other two
        while(currDislikes[0] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1]))))
            {
                currDislikes[0] = ol.GetClue(clueToTrySaving);
            }
        }
        
        //Get a fourth clue different than the other three
        while (currDislikes[1] == null)
        {
            clueToTrySaving = WeighRandomNumber(maxRandomNumber);
            if ((!ol.GetClue(clueToTrySaving).Equals(currLikes[0]) && (!ol.GetClue(clueToTrySaving).Equals(currLikes[1])) && (!ol.GetClue(clueToTrySaving).Equals(currDislikes[0]))))
            {
                currDislikes[1] = ol.GetClue(clueToTrySaving);
            }
        }

        //Display the order
        PrintOrder();
    }

    /// <summary>
    /// Used for debugging
    /// Displays the made order in the console
    /// </summary>
    public void PrintOrder()
    {
        print("Likes:\n" + currLikes[0] + "\n" + currLikes[1] + "\nDislikes:\n" + currDislikes[0] + "\n" + currDislikes[1]);

        print("Objects for Like #1:" + ol.GetObjectsForClueString(currLikes[0]));

    }

    /// <summary>
    /// Generates a random number and uses a weighing system to return a clue index
    /// </summary>
    /// <param name="max">The max random number</param>
    /// <returns>A clue index as an int</returns>
    public int WeighRandomNumber(int max)
    {
        //Generate a random number
        int unweighted = Random.Range(0, max);

        //Get a reference to the weighted array
        int[,] weightedValues = ol.WeightedArray();

        //Weigh the random number to get a weighed result
        for(int i=0; i<ol.data.Length; i++)
        {
            if(weightedValues[i,0]<= unweighted && unweighted < weightedValues[i,1])
            {
                return i;
            }

        }

        //Return 0 if nothing was caught
        return 0;
    }

    /// <summary>
    /// Checks the current order against the criteria and scores it
    /// </summary>
    /// <param name="choices">The dream the player made</param>
    /// <returns>Score</returns>
    public int CheckOrder(Constants.Objects[] choices)
    {
        int score = 0;
        Constants.Objects[] checkMe;

        //Checks each choice against the criteria
        for(int i=0; i< choices.Length; i++)
        {
            //Checks it against the likes
            for(int k=0; k<2; k++)
            {
                //Gets the accepted objects for the current clue
                checkMe = ol.GetObjectsForClue(currLikes[k]);
                for (int j = 0; j < checkMe.Length; j++)
                {
                    //If they match, increase score
                    if (checkMe[j] == choices[i])
                    {
                        score++;
                    }
                }
            }

            //Checks it against the dislikes
            for (int k = 0; k < 2; k++)
            {
                //Gets the hated objects for the current clue
                checkMe = ol.GetObjectsForClue(currDislikes[k]);
                for (int j = 0; j < checkMe.Length; j++)
                {
                    //If they match, decrease score
                    if (checkMe[j] == choices[i])
                    {
                        score--;
                    }
                }
            }
        }

        //Return the result
        return score;
        
    }

}
