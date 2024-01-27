/*****************************************************************************
// File Name :         Objects.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Links the name of objects with their image. Weighs their spawn chance. 

*****************************************************************************/
using UnityEngine;

[System.Serializable]
public class Objects
{
    [Header("Linking objects and Images")]
    public Constants.Objects name;
    public Sprite visual;


    [Header("Weight for Refilling")]
    [Range(1, 10)]
    public int weight;
}
