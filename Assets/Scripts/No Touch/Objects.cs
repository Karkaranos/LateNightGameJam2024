using System.Collections;
using System.Collections.Generic;
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
