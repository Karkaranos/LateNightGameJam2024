/*****************************************************************************
// File Name :         ObjectHandler.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Handles 

*****************************************************************************/
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public Objects[] items;
    private int max;

    /// <summary>
    /// Calculates the maximum number to randomly generate
    /// </summary>
    /// <returns></returns>
    public int TotalWeights()
    {
        int total = 0;
        for (int i = 0; i < items.Length; i++)
        {
            total += (11-items[i].weight);
        }

        return total;
    }

    /// <summary>
    /// Creates a weighted array of Objects based on their individual weights
    /// </summary>
    /// <returns>Returns a 2D int Array that stores weights</returns>
    public int[,] WeightedArray()
    {
        int[,] result = new int[items.Length, 2];
        int counter = 0;

        //Generates the weighted array
        for (int i = 0; i < items.Length; i++)
        {
            result[i, 0] = counter;
            counter += (11 - items[i].weight);
            result[i, 1] = counter;
        }

        //Returns the result
        return result;
    }

    /// <summary>
    /// Generates a random number and returns the weighted index of the result
    /// </summary>
    /// <returns>Index of object to generate</returns>
    public int WeighRandomNumber()
    {
        max = TotalWeights();
        //Generates a random number
        int unweighted = Random.Range(0, max);

        //Generates a weighted array
        int[,] weightedValues = WeightedArray();

        //Weighs the random number and returns the result
        for (int i = 0; i < items.Length; i++)
        {
            if (weightedValues[i, 0] <= unweighted && unweighted < weightedValues[i, 1])
            {
                return i;
            }

        }

        return 0;
    }
}
