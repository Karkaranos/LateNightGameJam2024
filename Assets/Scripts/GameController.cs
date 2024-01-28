/*****************************************************************************
// File Name :         GameController.cs
// Author :            Cade R. Naylor
// Creation Date :     January 26, 2024
//
// Brief Description : Creates a game controller. Handles shelving objects, scoring, and 
                        spawning new objects

*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
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
    public int[] cItems;
    [SerializeField]
    private int maxDupeItems;

    [Header("UI")]
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private TMP_Text roundEndText;
    [SerializeField]
    private TMP_Text failText;
    [SerializeField]
    private GameObject winCanvas;
    [SerializeField]
    private GameObject loseCanvas;
    [SerializeField]
    private GameObject gameCanvas;
    [SerializeField]
    private TMP_Text quotaText;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private GameObject scoreAdd;
    [SerializeField]
    private TMP_Text winScore;
    [SerializeField]
    private TMP_Text loseScore;
    [SerializeField]
    private GameObject notebook;

    [HideInInspector]
    public int currentObjects;

    [HideInInspector]
    public bool roundEnd = false;


    private ObjectHandler oh;
    private OrderHandler orh;
    private AudioManager am;
    private Timer timer;

    //References to input
    private PlayerInput mouseControls;
    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePos;
    private InputAction escape;
    private Vector2 currPos;

    private GameObject currentlyGrabbed;

    private string failCounter = "";

    private int days = 1;

    private int GRID_SIZE = 3;

    [Header("End Conditions and Scoring")]
    [SerializeField]
    private float dayTime;
    [SerializeField]
    private int dailyQuotaOfGood;
    [SerializeField]
    private int daysPlayed;
    private int currQuota;
    [SerializeField]
    private float moneyForDreamFilled;
    [SerializeField]
    private float moneyForNightmareFilled;
    [SerializeField]
    private float quotaFilledMultiplier;
    private float totalMoneyEarned;
    private float dailyMoneyEarned;


    [SerializeField] private string _filePath;
    public string[] dreamerNames;

    public GameObject[] ShelvesVis { get => shelvesVis; set => shelvesVis = value; }
    public float DayTime { get => dayTime;}

    #endregion


    #region Functions
    /// <summary>
    /// Start is called before the first frame update. 
    /// It gets references to other scripts, adds references to inputs, and initializes the shelves
    /// </summary>
    private void Start()
    {
        dreamerNames = File.ReadAllLines(Application.streamingAssetsPath + _filePath)[0].Split(",");
        oh = FindObjectOfType<ObjectHandler>();
        orh = FindObjectOfType<OrderHandler>();
        am = FindObjectOfType<AudioManager>();
        timer = FindObjectOfType<Timer>();

        mouseControls = GetComponent<PlayerInput>();
        mouseControls.currentActionMap.Enable();

        leftClick = mouseControls.currentActionMap.FindAction("Left Click");
        rightClick = mouseControls.currentActionMap.FindAction("Right Click");
        mousePos = mouseControls.currentActionMap.FindAction("MousePos");
        escape = mouseControls.currentActionMap.FindAction("Exit");

        leftClick.started += LeftClick_started;
        leftClick.canceled += LeftClick_canceled;
        escape.performed += Quit;
        //rightClick.performed += RightClick_performed;


        Vector3 spawnPos = topRightSpawnPos;
        spawnPos.z = 0;

        //Spawn the shelves
        int counter = 0;


        for(int i=0; i<GRID_SIZE; i++)
        {
            spawnPos.x = topRightSpawnPos.x;
            for(int j=0; j< GRID_SIZE; j++)
            {
                ShelvesVis[counter] = Instantiate(emptyPlace, spawnPos, Quaternion.identity);
                spawnPos.x += spaceBetweenSpots;
                counter++;
            }
            spawnPos.y -= spaceBetweenSpots;
        }

        //Set shelf references
        for(int i=0; i< GRID_SIZE * GRID_SIZE; i++)
        {
            shelfSpot[i] = ShelvesVis[i].transform.position;
            ShelvesVis[i].GetComponent<ConstantStorage>().index = i;
        }


        cItems = new int[GRID_SIZE * GRID_SIZE];
        for(int i=0; i<cItems.Length; i++)
        {
            cItems[i] = oh.items.Length + 5;
        }

        for(int i=0; i<GRID_SIZE * GRID_SIZE; i++)
        {
            GameObject temp = ShelvesVis[i];
            Destroy(temp);
        }

        StartCoroutine(StartDay());



    }

    private void Quit(InputAction.CallbackContext obj)
    {
        Application.Quit();
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
        /*if (am != null)
        {
            am.PlayClick();
        }*/
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

    IEnumerator StartDay()
    {
        if (am != null)
        {
            am.PlayNightMusic(days);
        }
        notebook.SetActive(false);
        roundEndText.text = "Night " + ToText(days) + " of your Dream Job";
        yield return new WaitForSeconds(3f);
        roundEndText.text = "";
        //Populate shelves with objects
        RefillAllShelves();
        orh.CreateNewOrder();
        timer.StartTimer();
        quotaText.text = "Your Progress: " + currQuota + " / " + dailyQuotaOfGood;
        scoreText.text = "Daily Earnings: $" + dailyMoneyEarned % 100;
        notebook.SetActive(true);
    }

    private string ToText(int num)
    {
        string s;
        switch (num)
        {
            case 1:
                s = "One";
                break;
            case 2:
                s = "Two";
                break;
            case 3:
                s = "Three";
                break;
            case 4:
                s = "Four";
                break;
            case 5:
                s = "Five";
                break;
            case 6:
                s = "Six";
                break;
            case 7:
                s = "Seven";
                break;
            case 8:
                s = "Eight";
                break;
            case 9:
                s = "Nine";
                break;
            case 10:
                s = "Ten";
                break;
            default:
                s = "???";
                break;
        }
        return s;
     
    }

    /// <summary>
    /// Refills all of the shelves with random objects
    /// </summary>
    private void RefillAllShelves()
    {
        int counter = 0;
        int dupesFound = 0;


        for (int i = 0; i < GRID_SIZE * GRID_SIZE; i++)
        {
            ShelvesVis[i] = Instantiate(emptyPlace, shelfSpot[i], Quaternion.identity);

        }

        while (counter < 9)
        {

            dupesFound = 0;
            int saveMe = oh.WeighRandomNumber();
            for(int i=0; i<cItems.Length; i++)
            {
                if(cItems[i] == saveMe)
                {
                    dupesFound++;
                }
            }
            if(dupesFound < maxDupeItems)
            {
                shelvesCont[counter] = oh.items[saveMe];
                ShelvesVis[counter].GetComponent<SpriteRenderer>().sprite = shelvesCont[counter].visual;
                ShelvesVis[counter].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                ShelvesVis[counter].GetComponent<ConstantStorage>().index = counter;
                cItems[counter] = saveMe;
                counter++;
            }
           
        }

        orh.CreateNewOrder();

    }

    /// <summary>
    /// Refills a specified number of objects
    /// </summary>
    /// <param name="newItems">The number of objects to refill</param>
    private void RefillObjects(int newItems)
    {
        currentObjects = 0;
        int[] emptyNums = new int[GRID_SIZE* GRID_SIZE];
        int counter = 0;
        int dupesFound = 0;

        //Restore existing items to their shelf locations
        for (int i=0; i< GRID_SIZE * GRID_SIZE; i++)
        {
            if(ShelvesVis[i] != null)
            {
                ShelvesVis[i].transform.position = shelfSpot[i];
                currentObjects++;
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
        for(int i=0; i<newItems; i++)
        {
            bool spawnedYet = false;
            for (int j=0; j< GRID_SIZE * GRID_SIZE; j++)
            {
                if (ShelvesVis[j]==null && !spawnedYet)
                {
                    dupesFound = 0;
                    int saveMe = oh.WeighRandomNumber();

                    for (int k = 0; k < cItems.Length; k++)
                    {
                        if (cItems[k] == saveMe)
                        {
                            dupesFound++;
                        }
                    }
                    if (dupesFound < maxDupeItems)
                    {
                        //Spawn the object and give it an appearance
                        ShelvesVis[j] = Instantiate(emptyPlace);
                        ShelvesVis[j].transform.position = shelfSpot[j];
                        shelvesCont[j] = oh.items[saveMe];
                        ShelvesVis[j].GetComponent<SpriteRenderer>().sprite = shelvesCont[j].visual;
                        ShelvesVis[j].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                        ShelvesVis[j].GetComponent<ConstantStorage>().index = j;
                        cItems[j] = saveMe;                        
                        counter++;
                        spawnedYet = true;
                        currentObjects++;
                    }
                }
            }
        }

        if(currentObjects>0)
        {
            orh.CreateNewOrder();
        }

        /*
        if(currentObjects >=3)
        {
            orh.CreateNewOrder();
        }
        else
        {
            //button.SetActive(true);
            roundEnd = true;
        }*/


    }



    /// <summary>
    /// Takes in the dream result and determines how many new items to add
    /// </summary>
    /// <param name="result"></param>
    public void HandleResults(int result)
    {
        currentObjects = 0;

        for (int i = 0; i < GRID_SIZE * GRID_SIZE; i++)
        {
            if (ShelvesVis[i] != null)
            {
                currentObjects++;
            }
        }


        if (currentObjects == 0)
        {
            RefillAllShelves();
        }

        else
        {
            int count = 0;
            if (result >= orh.NumOfLikes + 1)
            {
                count = 3;
                RefillObjects(count);
            }
            else if (result >= orh.NumOfLikes -1)
            {
                count = 2;
                RefillObjects(count);
            }
            else if (result > -1)
            {
                count = 1;
                RefillObjects(count);
            }
            else
            {
                RefillObjects(0);
            }



            print("results processed. You get " + count + " new ingredients.");


        }

        if (result >= 0)
        {
            currQuota++;
            if (currQuota < dailyQuotaOfGood)
            {
                quotaText.text = "Your Progress: " + currQuota + " / " + dailyQuotaOfGood;
            }
            else
            {
                quotaText.text = "Daily Quota Met!";
            }
        }

        float newMoney = 0;

        if ((result >= 0 && currQuota < dailyQuotaOfGood) || (result == 0 && currQuota >= dailyQuotaOfGood))
        {
            newMoney = moneyForDreamFilled;
            if (am != null)
            {
                am.DreamFeedback();
            }
        }
        else if (result > 0 && currQuota >= dailyQuotaOfGood)
        {
            newMoney = moneyForDreamFilled * quotaFilledMultiplier;
            if (am != null)
            {
                am.DreamFeedback();
            }
        }
        else if (result < 0)
        {
            newMoney = moneyForNightmareFilled;
            if (am != null)
            {
                am.NightmareFeedback();
            }
        }

        dailyMoneyEarned += newMoney;

        scoreText.text = "Daily Earnings: $" + (int)dailyMoneyEarned;

        if(am!=null)
        {
            am.KaCHING();
        }

        Vector3 spawnPos = scoreText.transform.position;
        spawnPos.y += Screen.height / 300;

        GameObject temp = Instantiate(scoreAdd, spawnPos, Quaternion.identity, gameCanvas.transform);
        temp.GetComponent<PointsBehavior>().Amount = (int)newMoney;



    }


    private void WinGame()
    {
        gameCanvas.SetActive(false);
        winCanvas.SetActive(true);
        winScore.text = "Your Earnings: $" + totalMoneyEarned;

    }

    private void LoseGame()
    {
        gameCanvas.SetActive(false);
        loseCanvas.SetActive(true);
        loseScore.text = "Your Earnings: $" + totalMoneyEarned;
    }

    public void RoundEndFunc()
    {
        StartCoroutine(PRIVATERoundEnd());
    }

    IEnumerator PRIVATERoundEnd()
    {
        for (int i = 0; i < GRID_SIZE * GRID_SIZE; i++)
        {
            GameObject temp = ShelvesVis[i];
            Destroy(temp);
        }

        totalMoneyEarned += dailyMoneyEarned;

        orh.ClearOrder();
        roundEndText.text = "Round Over";
        roundEnd = true;

        if (currQuota < dailyQuotaOfGood)
        {
            failCounter += 'X';
            if (am != null)
            {
                am.PlayFail();
            }
            failText.text = failCounter;
            if(failCounter.Equals("XXX"))
            {
                roundEndText.text = "Three Strikes, you're out!";
            }
        }
        yield return new WaitForSeconds(3f);
        roundEndText.text = "";
        scoreText.text = "";
        quotaText.text = "";
        dailyMoneyEarned = 0;

        currQuota = 0;
        dailyQuotaOfGood += 2;
        days++;
        if (failCounter.Equals("XXX"))
        {
            LoseGame();
        }
        else if (days > daysPlayed)
        {
            WinGame();
        }
        else
        {
            StartCoroutine(StartDay());
        }
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    #endregion 
}
