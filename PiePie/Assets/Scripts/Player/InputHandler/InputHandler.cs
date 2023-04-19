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
    public Vector2 InputVectorOnJetPack;
    public Vector2 InputVectorOnClimb;


    [Header("Bools")]
    [SerializeField] private bool run;
    [SerializeField] private bool jump;
    [SerializeField] private bool jumpOnBike;
    [SerializeField] public bool breakOnBike;
    [SerializeField] private bool jetPackRise;
    [SerializeField] private bool climbRightHand;
    [SerializeField] private bool climbLeftHand;
    [SerializeField] private bool climbRightFoot;
    [SerializeField] private bool climbLeftFoot;
    [SerializeField] private bool onground;
    [SerializeField] private bool climbing;

    public bool Jump { get { return jump; } set { jump = value; } }



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

        // MoveOnGround ActionMap
        PMBA.RegMove.Move.performed += MoveOnGround;
        PMBA.RegMove.Move.canceled += MoveOnGround;
        PMBA.RegMove.Jump.started += JumpOnGround;
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
        PMBA.JetpackMove.Jetpackmove.started += MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled += MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed += RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled += RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.performed += MoveOnClimb;
        PMBA.Climbmove.MoveHandsAndArms.canceled += MoveOnClimb;
        PMBA.Climbmove.RightHand.performed += RightHand;
        PMBA.Climbmove.RightHand.canceled += RightHand;
        PMBA.Climbmove.LeftHand.performed += LeftHand;
        PMBA.Climbmove.LeftHand.canceled += LeftHand;
        PMBA.Climbmove.RightFoot.performed += RightFoot;
        PMBA.Climbmove.RightFoot.canceled += RightFoot;
        PMBA.Climbmove.LeftFoot.performed += LeftFoot;
        PMBA.Climbmove.LeftFoot.canceled += LeftFoot;

    }

    //Disable Actions
    public void OnDisable()
    {
        PMBA.RegMove.Disable();
        PMBA.BikeMove.Disable();
        PMBA.JetpackMove.Disable();
        PMBA.Climbmove.Disable();

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
        PMBA.JetpackMove.Jetpackmove.started -= MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled -= MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed -= RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled -= RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.performed -= MoveOnClimb;
        PMBA.Climbmove.MoveHandsAndArms.canceled -= MoveOnClimb;
        PMBA.Climbmove.RightHand.performed -= RightHand;
        PMBA.Climbmove.RightHand.canceled -= RightHand;
        PMBA.Climbmove.LeftHand.performed -= LeftHand;
        PMBA.Climbmove.LeftHand.canceled -= LeftHand;
        PMBA.Climbmove.RightFoot.performed -= RightFoot;
        PMBA.Climbmove.RightFoot.canceled -= RightFoot;
        PMBA.Climbmove.LeftFoot.performed -= LeftFoot;
        PMBA.Climbmove.LeftFoot.canceled -= LeftFoot;
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

        run = true;

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

    public void RightHand(InputAction.CallbackContext ctx)
    {

        climbRightHand = ctx.ReadValueAsButton();

    }

    public void LeftHand(InputAction.CallbackContext ctx)
    {

        climbLeftHand = ctx.ReadValueAsButton();

    }

    public void RightFoot(InputAction.CallbackContext ctx)
    {

        climbRightFoot = ctx.ReadValueAsButton();

    }

    public void LeftFoot(InputAction.CallbackContext ctx)
    {

        climbLeftFoot = ctx.ReadValueAsButton();

    }
    private void Update()
    {
        //Debug.Log(InputVectorOnGround);
    }
   
}
