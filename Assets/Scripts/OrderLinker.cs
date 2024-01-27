using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLinker : MonoBehaviour
{
    public Clues[] data;

    public string GetClue(int id)
    {
        return data[id].clue;
    }

    public String GetObjectsForClue(string clue)
    {
        Constants.Objects[] result = new Constants.Objects[10];
        for(int i=0; i<data.Length-1; i++)
        {
            if (data[i].clue.Equals(clue))
            {
                result = data[i].associatedObjects;
            }
        }
        string s = "";
        foreach(Constants.Objects i in result)
        {
            s += i + " ";
        }
        return s;
    }



}
