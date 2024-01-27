using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] shelvesVis = new GameObject[9];
    Objects[] shelvesCont = new Objects[9];
    private Vector2[] shelfSpot = new Vector2[9];
    ObjectHandler oh;

    private PlayerInput mouseControls;

    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePos;

    private GameObject currentlyGrabbed;

    private int failCounter = 0;

    private Vector2 currPos;

    [SerializeField]
    private GameObject emptyPlace;

    private void Start()
    {
        oh = FindObjectOfType<ObjectHandler>();
        RefillAllShelves();

        mouseControls = GetComponent<PlayerInput>();
        mouseControls.currentActionMap.Enable();

        leftClick = mouseControls.currentActionMap.FindAction("Left Click");
        rightClick = mouseControls.currentActionMap.FindAction("Right Click");
        mousePos = mouseControls.currentActionMap.FindAction("MousePos");

        leftClick.started += LeftClick_started;
        leftClick.canceled += LeftClick_canceled;
        rightClick.performed += RightClick_performed;

        for(int i=0; i<9; i++)
        {
            shelfSpot[i] = shelvesVis[i].transform.position;
            shelvesVis[i].GetComponent<ConstantStorage>().index = i;
        }


    }

    private void LeftClick_canceled(InputAction.CallbackContext obj)
    {
        currentlyGrabbed = null;
    }

    private void LeftClick_started(InputAction.CallbackContext obj)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(currPos), Vector2.zero);

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

    private void Update()
    {
        currPos = mousePos.ReadValue<Vector2>();

        if(currentlyGrabbed!=null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(currPos);
            pos.z = 0;
            currentlyGrabbed.transform.position = pos;
        }
    }

    private void RightClick_performed(InputAction.CallbackContext obj)
    {
        print("Right Clicked");
    }

    private void RefillAllShelves()
    {
        int counter = 0;
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                int saveMe = oh.WeighRandomNumber();
                shelvesCont[counter] = oh.items[saveMe];
                shelvesVis[counter].GetComponent<SpriteRenderer>().sprite = shelvesCont[counter].visual;
                shelvesVis[counter].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                counter++;
            }
        }

    }

    private void RefillObjects(int newItems)
    {
        int[] emptyNums = new int[9];
        int counter = 0;
        for(int i=0; i<9; i++)
        {
            if(shelvesVis[i] != null)
            {
                shelvesVis[i].transform.position = shelfSpot[i];
            }
            else
            {
                emptyNums[counter] = i;
                counter++;
            }
        }

        counter = 0;


        for(int i=0; i<newItems; i++)
        {
            bool spawnedYet = false;
            for (int j=0; j<9; j++)
            {
                if (shelvesVis[j]==null && !spawnedYet)
                {
                    shelvesVis[j] = Instantiate(emptyPlace);
                    shelvesVis[j].transform.position = shelfSpot[j];
                    int saveMe = oh.WeighRandomNumber();
                    shelvesCont[j] = oh.items[saveMe];
                    shelvesVis[j].GetComponent<SpriteRenderer>().sprite = shelvesCont[j].visual;
                    shelvesVis[j].GetComponent<ConstantStorage>().itemName = oh.items[saveMe].name;
                    shelvesVis[j].GetComponent<ConstantStorage>().index = j;
                    counter++;
                    spawnedYet = true;
                }
            }
        }

    }




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



}
