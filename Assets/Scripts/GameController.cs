using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] shelvesVis = new GameObject[9];
    Objects[,] shelvesCont = new Objects[3, 3];
    ObjectHandler oh;

    private PlayerInput mouseControls;

    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction mousePos;

    private GameObject currentlyGrabbed;

    private Vector2 currPos;

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
                shelvesCont[i, j] = oh.items[oh.WeighRandomNumber()];
                shelvesVis[counter].GetComponent<SpriteRenderer>().sprite = shelvesCont[i, j].visual;
                counter++;
            }
        }

    }


}
