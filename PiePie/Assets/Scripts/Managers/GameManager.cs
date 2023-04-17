using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    PlayerMovementBigActionMap PMBA;

    [Header("Vehicles GameObjects")]
    public GameObject[] Vehicles;

    [Header("Has Items and Stuff")]
    public bool hasBag;
    public bool hasBike;
    public bool isReadyToClimb;
   

    [Header("Bools for Buttons InputSystem")]
    private bool startPressed;

    [Header("Indexs")]
    [SerializeField] private int currentVehicleIndex = 0;

    [Header("In NewState Thing")]
    [SerializeField] public bool isInClimbingArea;

    private void Awake()
    {
        PMBA = new PlayerMovementBigActionMap();
       
    }
    private void OnEnable()
    {
        PMBA.Interaction.Enable();

        PMBA.Interaction.Start.performed += StartMenu;
        PMBA.Interaction.Start.canceled += StartMenu;

        PMBA.Interaction.Accept.performed += acceptStuff;
        PMBA.Interaction.Accept.canceled += acceptStuff;

        PMBA.Interaction.SwitchVheicle.performed += SwitchVehicle;
        PMBA.Interaction.SwitchVheicle.canceled += SwitchVehicle;

    }

    private void OnDisable()
    {
        PMBA.Interaction.Start.performed -= StartMenu;
        PMBA.Interaction.Start.canceled -= StartMenu;

        PMBA.Interaction.Accept.performed -= acceptStuff;
        PMBA.Interaction.Accept.canceled -= acceptStuff;

        PMBA.Interaction.SwitchVheicle.performed -= SwitchVehicle;
        PMBA.Interaction.SwitchVheicle.canceled -= SwitchVehicle;

        PMBA.Interaction.Disable();
    }

   

    private void StartMenu(InputAction.CallbackContext ctx) // Checks if the player wants to pause the game
    {
        startPressed = true;
    }

    private void acceptStuff(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && isInClimbingArea)
        {
            currentVehicleIndex = 0;
            isReadyToClimb = true;
        }
       
    }

    private void SwitchVehicle(InputAction.CallbackContext ctx) // Looks in the index on which Vehicle will be active
    {
        if (ctx.performed)
        {
            // Increment Vehicle index
            currentVehicleIndex++;

            // If index exceeds array length, reset to 0
            if (currentVehicleIndex >= Vehicles.Length)
            {
                currentVehicleIndex = 0;
            }

            // Switch Vehicle
            SwitchVehicleOrder(currentVehicleIndex);
        }
    }

    public void SwitchVehicleOrder(int vehIndex) //Vehicle Swither Method
    {
        for (int i = 0; i < Vehicles.Length; i++)
        {
            Vehicles[i].SetActive(i == vehIndex);
        }
    }

    private void Update()
    {
      
    }
}
