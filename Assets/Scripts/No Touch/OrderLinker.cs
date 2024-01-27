/*****************************************************************************
// File Name :         OrderLinker.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Stores Clues and handles reporting their values

*****************************************************************************/
using UnityEngine;

public class OrderLinker : MonoBehaviour
{
    public Clues[] data;

    /// <summary>
    /// Given an ID value, returns the clue as a string
    /// </summary>
    /// <param name="id">index of clue</param>
    /// <returns>Clue in words</returns>
    public string GetClue(int id)
    {
        return data[id].Clue;
    }

    /// <summary>
    /// Finds all object matches given a clue as a string.
    /// Used for debugging
    /// </summary>
    /// <param name="clue">The clue to find</param>
    /// <returns>Reports the result as a string</returns>
    public string GetObjectsForClueString(string clue)
    {
        Constants.Objects[] result = new Constants.Objects[10];

        //Gets a reference to all objects associated with the provided clue
        for(int i=0; i<data.Length-1; i++)
        {
            if (data[i].Clue.Equals(clue))
            {
                result = data[i].AssociatedObjects;
            }
        }

        //Converts the references into a string
        string s = "";
        foreach(Constants.Objects i in result)
        {
            s += i + " ";
        }

        //Returns the string
        return s;
    }

    /// <summary>
    /// Finds all object matches given a clue as a string. 
    /// </summary>
    /// <param name="clue">The clue to find</param>
    /// <returns>An array of matching objects</returns>
    public Constants.Objects[] GetObjectsForClue(string clue)
    {
        Constants.Objects[] result = new Constants.Objects[10];

        //Finds all objects that match the provided clue
        for (int i = 0; i < data.Length - 1; i++)
        {
            if (data[i].Clue.Equals(clue))
            {
                result = data[i].AssociatedObjects;
            }
        }

        //Returns the result
        return result;
    }

    /// <summary>
    /// Calculates the maximum number to generate for weighing numbers
    /// </summary>
    /// <returns>The max number to generate as an int</returns>
    public int TotalWeights()
    {
        int total = 0;
        
        //Calculates weighted max
        for(int i=0; i<data.Length; i++)
        {
            total += (11-data[i].Weight);
        }

        //Returns the result
        return total;
    }

    /// <summary>
    /// Generates a weighted array 
    /// </summary>
    /// <returns>Returns the result as a 2D int Array</returns>
    public int[,] WeightedArray()
    {
        int[,] result = new int[data.Length,2];
        int counter = 0;

        //Generate the weighted array
        for(int i=0; i<data.Length; i++)
        {
            result[i,0] = counter;
            counter += (11-data[i].Weight);
            result[i,1] = counter;
        }

        //Return the result
        return result;
    }
}
