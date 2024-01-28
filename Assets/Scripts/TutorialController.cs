/*****************************************************************************
// File Name :         TutorialController.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Creates a tutorial game controller. Handles shelving objects, 
                        shelving objects, and spawning new objects.

*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    #region Variables
    [Header("Tutorial Specific")]
    [SerializeField]
    private GameObject startButton;
    public int tutorialStage = 1;

    [Header("Object Storage and Generation")]
    [SerializeField]
    private Vector2 topRightSpawnPos;
    [SerializeField]
    private float spaceBetweenSpots;
    [SerializeField]
    private GameObject emptyPlace;
    [SerializeField] GameObject[] shelvesVis = new GameObject[9];
    Objects[] shelvesCont = new Objects[9];
    private Vector2[] shelfSpot = new Vector2[9];

    [Header("UI")]
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject tutorial;
    [SerializeField]
    private TMP_Text tutorialText;
    [SerializeField]
    private GameObject tutButton;
    [SerializeField]
    private GameObject list;

    private ObjectHandler oh;
    private OrderHandler orh;
    private AudioManager am;

    private bool canDrag = false;

    //References to input
    private PlayerInput mouseControls;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePos;
    private Vector2 currPos;

    private GameObject currentlyGrabbed;

    [SerializeField]
    private string[] lines;
    private int lineNum;

    public GameObject[] ShelvesVis { get => shelvesVis; set => shelvesVis = value; }

    private int GRID_SIZE = 3;

    #endregion

    #region Functions

    /// <summary>
    /// Start is called before the first frame update. 
    /// It gets references to other scripts, adds references to inputs, and initializes the shelves
    /// </summary>
    private void Start()
    {
        oh = FindObjectOfType<ObjectHandler>();
        orh = FindObjectOfType<OrderHandler>();
        am = FindObjectOfType<AudioManager>();

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


        for (int i = 0; i < GRID_SIZE; i++)
        {
            spawnPos.x = topRightSpawnPos.x;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                ShelvesVis[counter] = Instantiate(emptyPlace, spawnPos, Quaternion.identity);
                spawnPos.x += spaceBetweenSpots;
                counter++;
            }
            spawnPos.y -= spaceBetweenSpots;
        }

        //Set shelf references
        for (int i = 0; i < GRID_SIZE*GRID_SIZE; i++)
        {
            shelfSpot[i] = ShelvesVis[i].transform.position;
            ShelvesVis[i].GetComponent<ConstantStorage>().index = i;
        }

        for(int i = 0; i < shelvesVis.Length; i++)
        {
            GameObject temp = shelvesVis[i];
            Destroy(temp);
        }

        RefillShelves();

        tutorialText.text = lines[0];
    }

    private void OnDisable()
    {
        leftClick.started -= LeftClick_started;
        leftClick.canceled -= LeftClick_canceled;

        //mouseControls.currentActionMap.Disable();
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
        /*if (am != null)
        {
            am.PlayClick();
        }*/
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currPos), Vector2.zero);
        if(canDrag)
        {
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
    }

    /// <summary>
    /// Update occurs every frame. Reads mouse position and sets grabbed ingredient's position
    /// </summary>
    private void Update()
    {
        currPos = mousePos.ReadValue<Vector2>();

        //If an object is being dragged
        if (currentlyGrabbed != null)
        {
            //Converts screen space into world space 
            Vector3 pos = Camera.main.ScreenToWorldPoint(currPos);
            pos.z = 0;
            currentlyGrabbed.transform.position = pos;
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        if (am != null)
        {
            am.PlayClick();
        }
        SceneManager.LoadScene("GameScene");
    }

    public void RefillShelves()
    {
        if(tutorialStage == 0)
        {
            int currIndex = 0;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[1];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[1].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 2;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[1];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[1].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 3;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[5];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[5].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 4;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[4];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[4].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 8;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[2];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[2].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;
        }

        if(tutorialStage == 1)
        {
            for (int i = 0; i < shelvesVis.Length; i++)
            {
                GameObject temp = shelvesVis[i];
                Destroy(temp);
            }

            int currIndex = 0;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[2];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[2].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 3;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[7];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[7].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 4;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[3];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[3].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 6;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[5];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[5].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 7;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[6];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[6].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;

            currIndex = 8;
            ShelvesVis[currIndex] = Instantiate(emptyPlace);
            ShelvesVis[currIndex].transform.position = shelfSpot[currIndex];
            shelvesCont[currIndex] = oh.items[3];
            ShelvesVis[currIndex].GetComponent<SpriteRenderer>().sprite = shelvesCont[currIndex].visual;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().itemName = oh.items[3].name;
            ShelvesVis[currIndex].GetComponent<ConstantStorage>().index = currIndex;
        }


    }

    public void IncreaseTutorial()
    {
        tutorialStage++;
        tutButton.SetActive(true);
        orh.CreateNewTutorialOrder();
        TutorialText();
    }

    public void TutorialNextPressed()
    {
        if (am != null)
        {
            am.PlayClick();
        }
        TutorialText();
    }

    public void TutorialText()
    {
        lineNum++;
        if (lines[lineNum+1].Equals(""))
        {
            tutButton.SetActive(false);
            canDrag = true;
        }
        if(lineNum == 6)
        {
            canDrag = false;
        }
        if(lineNum == 7)
        {
            RefillShelves();
        }
        if(lineNum==10)
        {
            canDrag = false;
            for (int i = 0; i < shelvesVis.Length; i++)
            {
                GameObject temp = shelvesVis[i];
                Destroy(temp);
            }
            list.SetActive(false);
            
        }
        tutorialText.text = lines[lineNum];
    }

    /// <summary>
    /// Takes in the dream result and determines how many new items to add
    /// </summary>
    /// <param name="result"></param>
    public void HandleResults(int result)
    {
        if(tutorialStage==0)
        {
            if (am != null)
            {
                am.DreamFeedback();
            }
            RefillObjects(3);
            lineNum = 5;
        }
        else if (tutorialStage == 1)
        {
            if (am != null)
            {
                am.NightmareFeedback();
            }
            lineNum = 9;
        }
        IncreaseTutorial();
    }

    /// <summary>
    /// Refills a specified number of objects
    /// </summary>
    /// <param name="newItems">The number of objects to refill</param>
    private void RefillObjects(int newItems)
    {
        int[] emptyNums = new int[GRID_SIZE * GRID_SIZE];
        for(int i=0; i<emptyNums.Length; i++)
        {
            emptyNums[i] = 100;
        }
        int counter = 0;
        int dupesFound = 0;

        //Restore existing items to their shelf locations
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; i++)
        {
            if (ShelvesVis[i] != null)
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

        //counter = 0;

        //Fill the blank spaces...if the number of new items added allows
        for (int i = 0; i < newItems; i++)
        {
            bool spawnedYet = false;
            for (int j = 0; j < GRID_SIZE * GRID_SIZE; j++)
            {
                if (ShelvesVis[j] == null && !spawnedYet)
                {
                    int saveMe = oh.WeighRandomNumber();

                    ShelvesVis[j] = Instantiate(emptyPlace);
                    ShelvesVis[j].transform.position = shelfSpot[j];
                    shelvesCont[j] = oh.items[saveMe];
                    ShelvesVis[j].GetComponent<SpriteRenderer>().sprite = shelvesCont[j].visual;
                    ShelvesVis[j].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                    counter++;
                    spawnedYet = true;
                }
            }
        }


    }



    #endregion
}
