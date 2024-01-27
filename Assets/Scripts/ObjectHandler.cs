using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public Objects[] items;
    int max;

    public int TotalWeights()
    {
        int total = 0;
        for (int i = 0; i < items.Length - 1; i++)
        {
            total += items[i].weight;
        }
        return total;
    }

    public int[,] WeightedArray()
    {
        max = TotalWeights();
        int[,] result = new int[items.Length, 2];
        int counter = 0;

        for (int i = 0; i < items.Length - 1; i++)
        {
            result[i, 0] = counter;
            counter += (11 - items[i].weight);
            result[i, 1] = counter;
        }
        return result;
    }

    public int WeighRandomNumber()
    {
        int unweighted = Random.Range(0, max);

        int[,] weightedValues = WeightedArray();

        for (int i = 0; i < items.Length - 1; i++)
        {
            if (weightedValues[i, 0] <= unweighted && unweighted < weightedValues[i, 1])
            {
                return i;
            }

        }

        return 0;
    }
}
