using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clues
{
    public string clue;
    public Constants.Objects[] associatedObjects;
    [Range(1, 10)]
    public int weight;
}
