/*****************************************************************************
// File Name :         OrderHandler.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Creates orders following a weighted random system. No clue may 
                        be used twice per order. Checks if the order is correct.

*****************************************************************************/
using UnityEngine;
using TMPro;

public class OrderHandler : MonoBehaviour
{
    private OrderLinker ol;
    private string[] currLikes;
    private string[] currDislikes;
    private int maxRandomNumber;

    private int[] allClues;

    [Header("Order Size")]
    [Range(0, 3)][SerializeField]
    private int numOfLikes;
    [Range(0, 3)]
    [SerializeField]
    private int numOfDislikes;

    [Header("Visual Aspects")]
    [SerializeField]
    private TMP_Text listText;
    [SerializeField]
    private TMP_Text dreamerNameText;

    GameController gc;
    TutorialController tc;

    public int NumOfLikes { get => numOfLikes;}


    /// <summary>
    /// Start is called before the first frame update. It gets references to other scripts and creates an order. 
    /// </summary>
    void Start()
    {
        ol = FindObjectOfType<OrderLinker>();
        tc = FindObjectOfType<TutorialController>();
        gc = FindObjectOfType<GameController>();

        
        maxRandomNumber = ol.TotalWeights();

        currLikes = new string[NumOfLikes];
        currDislikes = new string[numOfDislikes];

        allClues = new int[numOfDislikes + NumOfLikes];

        if(tc!=null)
        {
            CreateNewTutorialOrder();
        }
    }

    /// <summary>
    /// Creates a new order using weighted random numbers
    /// </summary>
    public void CreateNewOrder()
    {
        //print("new order made");
        //Create the first clue
        int clueToTrySaving = WeighRandomNumber(maxRandomNumber);
        /*
        currLikes[0] = ol.GetClue(clueToTrySaving);
        allClues[0] = */

        bool dupeFound = false;
        int counter = 0;

        for(int i=0; i<currLikes.Length; i++)
        {
            while(currLikes[i]==null)
            {
                dupeFound = false;
                clueToTrySaving = WeighRandomNumber(maxRandomNumber);
                for(int j=0; j<allClues.Length; j++)
                {
                    if(allClues[j] == clueToTrySaving)
                    {
                        dupeFound = true;
                    }
                }
                if(!dupeFound)
                {
                    currLikes[i] = ol.GetClue(clueToTrySaving);
                    allClues[counter] = clueToTrySaving;
                    counter++;
                }
            }
        }

        for (int i = 0; i < currDislikes.Length; i++)
        {
            while (currDislikes[i] == null)
            {
                dupeFound = false;
                clueToTrySaving = WeighRandomNumber(maxRandomNumber);
                for (int j = 0; j < allClues.Length; j++)
                {
                    if (allClues[j] == clueToTrySaving)
                    {
                        dupeFound = true;
                    }
                }
                if (!dupeFound)
                {
                    currDislikes[i] = ol.GetClue(clueToTrySaving);
                    allClues[counter] = clueToTrySaving;
                    counter++;
                }
            }
        }

        //Display the order
        PrintOrder();
    }

    public void ClearOrder()
    {
        dreamerNameText.text = "";
        listText.text = "";
    }

    public void CreateNewTutorialOrder()
    {
        if(tc.tutorialStage == 0)
        {
            currLikes[0] = "Baking";
            currLikes[1] = "Green";
            currDislikes[0] = "Yellow";
            currDislikes[1] = "Sport";
        }

        if (tc.tutorialStage == 1)
        {
            currLikes[0] = "Buildings";
            currLikes[1] = "Feathery";
            currDislikes[0] = "Food";
            currDislikes[1] = "Sport";
        }

        PrintOrder();
    }


    /// <summary>
    /// Displays the made order
    /// </summary>
    public void PrintOrder()
    {
        string s = "Likes:\n";

        for(int i=0; i<currLikes.Length; i++)
        {
            s += "- " + currLikes[i] + "\n";
        }

        s += "Dislikes:\n";

        for (int i = 0; i < currDislikes.Length; i++)
        {
            s += "- " + currDislikes[i] + "\n";
        }

        //print(s);
        listText.text = s;

        if(gc!=null)
        {
            int name = (int)Random.Range(0, gc.dreamerNames.Length);
            dreamerNameText.text = gc.dreamerNames[name];
        }
        else if (tc!=null)
        {
            if(tc.tutorialStage == 0)
            {
                dreamerNameText.text = "Zach";
            }
            if(tc.tutorialStage == 1)
            {
                dreamerNameText.text = "Gorp";
            }
        }
        else
        {
            //print("no controllers found");
        }



        //print("Likes:\n" + currLikes[0] + "\n" + currLikes[1] + "\nDislikes:\n" + currDislikes[0] + "\n" + currDislikes[1]);

        //print("Objects for Like #1:" + ol.GetObjectsForClueString(currLikes[0]));

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
            for(int k=0; k<currLikes.Length; k++)
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
            for (int k = 0; k < currDislikes.Length; k++)
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

        for(int i=0; i<currLikes.Length; i++)
        {
            currLikes[i] = null;
        }
        for (int i = 0; i < currDislikes.Length; i++)
        {
            currDislikes[i] = null;
        }

        //Return the result
        return score;
        
    }

}
