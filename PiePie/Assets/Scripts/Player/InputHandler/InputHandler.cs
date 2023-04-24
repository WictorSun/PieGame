using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerMovementBigActionMap PMBA;
    
    //MovementVectors
    public Vector2 InputVectorOnGround { get; private set; }
    public Vector2 InputVectorOnBike { get;  private set; }
    public Vector2 InputVectorOnBikeSpeed { get; private set; }
    public Vector2 InputVectorOnJetPack { get; private set; }
    public Vector2 InputVectorOnClimb { get; private set; }


    [Header("Bools")]
    [SerializeField] private bool run;
    [SerializeField] private bool jump;
    [SerializeField] private bool jumpOnBike;
    [SerializeField] public bool breakOnBike;
    [SerializeField] private bool jetPackRise;
   
    [SerializeField] private bool onground;
    [SerializeField] private bool climbing;
    [SerializeField] private bool _cancleClimbing;
    [SerializeField] private bool _switchVehicle;

    public bool Jump { get { return jump; } set { jump = value; } }
    public bool CanleClimbing { get { return _cancleClimbing; } set { _cancleClimbing = value; } }
    public bool SwitchVehicle1 { get { return _switchVehicle; } set { _switchVehicle = value; } }
    public bool Run { get { return run; } set { run = value; } }
    public bool Rise { get { return jetPackRise; } set { jetPackRise = value; } }

    public void Awake()
    {
        PMBA = new PlayerMovementBigActionMap();

      
    }




    //Enable Actions
    public void OnEnable()
    {
        PMBA.RegMove.Enable();
        PMBA.BikeMove.Enable();
        PMBA.JetpackMove.Enable();
        PMBA.Climbmove.Enable();
        PMBA.Interaction.Enable();

        // MoveOnGround ActionMap
        PMBA.RegMove.Move.performed += MoveOnGround;
        PMBA.RegMove.Move.canceled += MoveOnGround;
        PMBA.RegMove.Jump.performed += JumpOnGround;
        PMBA.RegMove.Jump.canceled += JumpOnGround;
        PMBA.RegMove.Run.started += RunOnGround;
        PMBA.RegMove.Run.canceled += RunOnGround;

        // Bike ActionMap
        PMBA.BikeMove.MoveBike.performed += MoveOnBike;
        PMBA.BikeMove.MoveBike.canceled += MoveOnBike;
        PMBA.BikeMove.JumpBike.performed += JumpOnBike;
        PMBA.BikeMove.JumpBike.canceled += JumpOnBike;
        PMBA.BikeMove.SpeedBike.performed += SpeedOnBike;
        PMBA.BikeMove.SpeedBike.canceled += SpeedOnBike;
        PMBA.BikeMove.Break.started += BreakOnBike;
        PMBA.BikeMove.Break.canceled += BreakOnBike;

        // JetPack ActionMap
        PMBA.JetpackMove.Jetpackmove.performed += MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled += MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed += RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled += RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.performed += MoveOnClimb;
        PMBA.Climbmove.MoveHandsAndArms.canceled += MoveOnClimb;
        PMBA.Climbmove.CancelClimb.performed += JumpOffClif;
        PMBA.Climbmove.CancelClimb.canceled += JumpOffClif;

        //Interact
        PMBA.Interaction.SwitchVheicle.started += SwitchVehicle;
        PMBA.Interaction.SwitchVheicle.canceled += SwitchVehicle;
        PMBA.Interaction.Start.performed += SwitchVehicle;
        PMBA.Interaction.Start.canceled += SwitchVehicle;
    }

    //Disable Actions
    public void OnDisable()
    {
        PMBA.RegMove.Disable();
        PMBA.BikeMove.Disable();
        PMBA.JetpackMove.Disable();
        PMBA.Climbmove.Disable();
        PMBA.Interaction.Disable();
        // MoveOnGround ActionMap
        PMBA.RegMove.Move.performed -= MoveOnGround;
        PMBA.RegMove.Move.canceled -= MoveOnGround;
        PMBA.RegMove.Jump.started -= JumpOnGround;
        PMBA.RegMove.Jump.canceled -= JumpOnGround;
        PMBA.RegMove.Run.started -= RunOnGround;
        PMBA.RegMove.Run.canceled -= RunOnGround;

        // Bike ActionMap
        PMBA.BikeMove.MoveBike.performed -= MoveOnBike;
        PMBA.BikeMove.MoveBike.canceled -= MoveOnBike;
        PMBA.BikeMove.JumpBike.performed -= JumpOnBike;
        PMBA.BikeMove.JumpBike.canceled -= JumpOnBike;
        PMBA.BikeMove.SpeedBike.performed -= SpeedOnBike;
        PMBA.BikeMove.SpeedBike.canceled -= SpeedOnBike;
        PMBA.BikeMove.Break.started -= BreakOnBike;
        PMBA.BikeMove.Break.canceled -= BreakOnBike;


        // JetPack ActionMap
        PMBA.JetpackMove.Jetpackmove.performed -= MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled -= MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed -= RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled -= RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.performed -= MoveOnClimb;
        PMBA.Climbmove.MoveHandsAndArms.canceled -= MoveOnClimb;
        PMBA.Climbmove.CancelClimb.performed -= JumpOffClif;
        PMBA.Climbmove.CancelClimb.canceled -= JumpOffClif;

        //Interact
        PMBA.Interaction.SwitchVheicle.started -= SwitchVehicle;
        PMBA.Interaction.SwitchVheicle.canceled -= SwitchVehicle;
        PMBA.Interaction.Start.performed -= SwitchVehicle;
        PMBA.Interaction.Start.canceled -= SwitchVehicle;
    }

    public void SwitchVehicle(InputAction.CallbackContext ctx)
    {
        _switchVehicle = ctx.ReadValueAsButton();
    }

    public void MoveOnGround(InputAction.CallbackContext ctx)
    {
        InputVectorOnGround = ctx.ReadValue<Vector2>();


    }

    public void JumpOnGround(InputAction.CallbackContext ctx)
    {

        jump = ctx.ReadValueAsButton();

    }

    public void RunOnGround(InputAction.CallbackContext ctx)
    {

        run = ctx.ReadValueAsButton();

    }

    public void MoveOnBike(InputAction.CallbackContext ctx)
    {

        InputVectorOnBike = ctx.ReadValue<Vector2>();

    }

    public void JumpOnBike(InputAction.CallbackContext ctx)
    {

        jumpOnBike = ctx.ReadValueAsButton();

    }

    public void SpeedOnBike(InputAction.CallbackContext ctx)
    {

        InputVectorOnBikeSpeed = ctx.ReadValue<Vector2>();

    }
    public void BreakOnBike(InputAction.CallbackContext ctx)
    {
        breakOnBike = ctx.ReadValueAsButton();
    }
    public void MoveOnJetPack(InputAction.CallbackContext ctx)
    {

        InputVectorOnJetPack = ctx.ReadValue<Vector2>();
    }

    public void RiseOnJetPack(InputAction.CallbackContext ctx)
    {

        jetPackRise = ctx.ReadValueAsButton();

    }

    public void MoveOnClimb(InputAction.CallbackContext ctx)
    {

        InputVectorOnClimb = ctx.ReadValue<Vector2>();

    }
    private void JumpOffClif(InputAction.CallbackContext ctx)
    {
        _cancleClimbing = ctx.ReadValueAsButton();
    }

    private void Update()
    {
        //Debug.Log(_switchVehicle);
    }
   
}
