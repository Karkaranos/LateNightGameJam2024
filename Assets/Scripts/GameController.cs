using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class GameController : MonoBehaviour
{
    #region Variables
    [Header("Object Storage and Generation")]
    [SerializeField]
    private Vector2 topRightSpawnPos;
    [SerializeField]
    private float spaceBetweenSpots;
    [SerializeField]
    private GameObject emptyPlace;
    private GameObject[] shelvesVis = new GameObject[9];
    Objects[] shelvesCont = new Objects[9];
    private Vector2[] shelfSpot = new Vector2[9];


    private ObjectHandler oh;

    //References to input
    private PlayerInput mouseControls;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePos;
    private Vector2 currPos;

    private GameObject currentlyGrabbed;

    private int failCounter = 0;

    public GameObject[] ShelvesVis { get => shelvesVis; set => shelvesVis = value; }

    #endregion


    #region Functions
    /// <summary>
    /// Start is called before the first frame update. 
    /// It gets references to other scripts, adds references to inputs, and initializes the shelves
    /// </summary>
    private void Start()
    {
        oh = FindObjectOfType<ObjectHandler>();

        mouseControls = GetComponent<PlayerInput>();
        mouseControls.currentActionMap.Enable();

        leftClick = mouseControls.currentActionMap.FindAction("Left Click");
        rightClick = mouseControls.currentActionMap.FindAction("Right Click");
        mousePos = mouseControls.currentActionMap.FindAction("MousePos");

        leftClick.started += LeftClick_started;
        leftClick.canceled += LeftClick_canceled;
        //rightClick.performed += RightClick_performed;


        Vector3 spawnPos = topRightSpawnPos;
        spawnPos.z = 0;

        //Spawn the shelves
        int counter = 0;
        for(int i=0; i<3; i++)
        {
            spawnPos.x = topRightSpawnPos.x;
            for(int j=0; j<3; j++)
            {
                ShelvesVis[counter] = Instantiate(emptyPlace, spawnPos, Quaternion.identity);
                spawnPos.x += spaceBetweenSpots;
                counter++;
            }
            spawnPos.y -= spaceBetweenSpots;
        }

        //Set shelf references
        for(int i=0; i<9; i++)
        {
            shelfSpot[i] = ShelvesVis[i].transform.position;
            ShelvesVis[i].GetComponent<ConstantStorage>().index = i;
        }

        //Populate shelves with objects
        RefillAllShelves();


    }

    private void OnDisable()
    {
        leftClick.started -= LeftClick_started;
        leftClick.canceled -= LeftClick_canceled;

        mouseControls.currentActionMap.Disable();
    }

    /// <summary>
    /// Removes the item currently being dragged when left click is stopped
    /// </summary>
    /// <param name="obj">Click being cancelled</param>
    private void LeftClick_canceled(InputAction.CallbackContext obj)
    {
        currentlyGrabbed = null;
    }

    /// <summary>
    /// Sends out a raycast to grab an ingredient on left click
    /// </summary>
    /// <param name="obj">Click started</param>
    private void LeftClick_started(InputAction.CallbackContext obj)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currPos), Vector2.zero);

        //Get a reference to the ingredient, if one was encountered
        try
        {
            if (hit.transform.gameObject.tag == "Ingredient")
            {
                currentlyGrabbed = hit.transform.gameObject;
            }
        }
        catch
        {
            //stop throwing nulls
        }
    }

    /// <summary>
    /// Update occurs every frame. Reads mouse position and sets grabbed ingredient's position
    /// </summary>
    private void Update()
    {
        currPos = mousePos.ReadValue<Vector2>();

        //If an object is being dragged
        if(currentlyGrabbed!=null)
        {
            //Converts screen space into world space 
            Vector3 pos = Camera.main.ScreenToWorldPoint(currPos);
            pos.z = 0;
            currentlyGrabbed.transform.position = pos;
        }
    }

    /// <summary>
    /// Refills all of the shelves with random objects
    /// </summary>
    private void RefillAllShelves()
    {
        int counter = 0;
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                int saveMe = oh.WeighRandomNumber();
                shelvesCont[counter] = oh.items[saveMe];
                ShelvesVis[counter].GetComponent<SpriteRenderer>().sprite = shelvesCont[counter].visual;
                ShelvesVis[counter].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                counter++;
            }
        }

    }

    /// <summary>
    /// Refills a specified number of objects
    /// </summary>
    /// <param name="newItems">The number of objects to refill</param>
    private void RefillObjects(int newItems)
    {
        int[] emptyNums = new int[9];
        int counter = 0;

        //Restore existing items to their shelf locations
        for(int i=0; i<9; i++)
        {
            if(ShelvesVis[i] != null)
            {
                ShelvesVis[i].transform.position = shelfSpot[i];
            }
            else
            //If an item doesn't exist, add it to the blank space array
            {
                emptyNums[counter] = i;
                counter++;
            }
        }

        counter = 0;

        //Fill the blank spaces...if the number of new items added allows
        for(int i=0; i<newItems; i++)
        {
            bool spawnedYet = false;
            for (int j=0; j<9; j++)
            {
                if (ShelvesVis[j]==null && !spawnedYet)
                {
                    //Spawn the object and give it an appearance
                    ShelvesVis[j] = Instantiate(emptyPlace);
                    ShelvesVis[j].transform.position = shelfSpot[j];
                    int saveMe = oh.WeighRandomNumber();
                    shelvesCont[j] = oh.items[saveMe];

                    //Set the object's variables
                    ShelvesVis[j].GetComponent<SpriteRenderer>().sprite = shelvesCont[j].visual;
                    ShelvesVis[j].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                    ShelvesVis[j].GetComponent<ConstantStorage>().index = j;
                    counter++;
                    spawnedYet = true;
                }
            }
        }

    }



    /// <summary>
    /// Takes in the dream result and determines how many new items to add
    /// </summary>
    /// <param name="result"></param>
    public void HandleResults(int result)
    {
        int count = 0;
        if(result >= 2)
        {
            count = 3;
            RefillObjects(count);
        }
        else if (result < 2 && result > -1)
        {
            count = 2;
            RefillObjects(count);
        }
        else if (result > -3)
        {
            count = 1;
            RefillObjects(count);
        }
        else
        {
            failCounter++;
        }
        print("results processed. You get " + count + " new ingredients.");
    }


    #endregion 
}
