/*****************************************************************************
// File Name :         Clues.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Creates the base class for clues which holds a clue string 
                        and its related objects. Visible in inspector. 

*****************************************************************************/
using UnityEngine;

[System.Serializable]
public class Clues
{

    [SerializeField]
    private string _clue;
    [SerializeField]
    private Constants.Objects[] _associatedObjects;

    [Header("Likelihood to be a Clue")]
    [Range(1, 10)]
    [SerializeField] private int weight;

    public string Clue { get => _clue;}
    public Constants.Objects[] AssociatedObjects { get => _associatedObjects; }
    public int Weight { get => weight;}
}
