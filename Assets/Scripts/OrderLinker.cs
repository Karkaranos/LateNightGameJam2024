using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLinker : MonoBehaviour
{
    public Clues[] data;

    public string GetClue(int id)
    {
        return data[id].Clue;
    }

    public string GetObjectsForClueString(string clue)
    {
        Constants.Objects[] result = new Constants.Objects[10];
        for(int i=0; i<data.Length-1; i++)
        {
            if (data[i].Clue.Equals(clue))
            {
                result = data[i].AssociatedObjects;
            }
        }
        string s = "";
        foreach(Constants.Objects i in result)
        {
            s += i + " ";
        }
        return s;
    }

    public Constants.Objects[] GetObjectsForClue(string clue)
    {
        Constants.Objects[] result = new Constants.Objects[10];
        for (int i = 0; i < data.Length - 1; i++)
        {
            if (data[i].Clue.Equals(clue))
            {
                result = data[i].AssociatedObjects;
            }
        }
        return result;
    }

    public int TotalWeights()
    {
        int total = 0;
        for(int i=0; i<data.Length; i++)
        {
            total += (11-data[i].Weight);
        }
        return total;
    }

    public int[,] WeightedArray()
    {
        int max = TotalWeights();
        int[,] result = new int[data.Length,2];
        int counter = 0;

        for(int i=0; i<data.Length; i++)
        {
            result[i,0] = counter;
            counter += (11-data[i].Weight);
            result[i,1] = counter;
        }
        //PrintArray(result);
        return result;
    }

    public void PrintArray(int[,] printMe)
    {
        string s = "Mins: ";
        for(int i=0; i<data.Length-1; i++)
        {
            s += printMe[i,0] + ", ";
        }
        s += "\nMaxs: ";
        for (int i = 0; i < data.Length - 1; i++)
        {
            s += printMe[i,1] + ", ";
        }
        print(s);
    }


}
