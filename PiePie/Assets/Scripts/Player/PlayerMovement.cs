using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference Other Scripts")]
    PlayerMovementBigActionMap PMBA;
    [SerializeField] private GameManager GM;

    [Header("Components")]
    [SerializeField] Rigidbody PlayerRb;
    [SerializeField] Animator playerAnim;
    private PlayerInput playerinput;

    [Header("Animation")]
    int moveAnimationID;
    int moveWithBagId;

    [Header("Bools")]
    [SerializeField] private bool run;
    [SerializeField] private bool jump;
    [SerializeField] private bool jumpOnBike;
    [SerializeField] private bool jetPackRise;
    [SerializeField] private bool climbRightHand;
    [SerializeField] private bool climbLeftHand;
    [SerializeField] private bool climbRightFoot;
    [SerializeField] private bool climbLeftFoot;
    [SerializeField] private bool onground;


    [Header("Floats")]
    [SerializeField] private float onGroundMoveForceSlow;
    [SerializeField] private float onGroundMoveForceNormal;
    [SerializeField] private float OnGroundRun;
    [SerializeField] private float rotationSpeed;

    [Header("Jumping")]
    public float jumpForce = 10f;
    public float maxJumpTime = 1f;
    public float fallMultiplier = 2.5f;
    private float jumpTimer = 0f;
    public int maxJumps = 2;
    public int jumpCount = 0;
    public bool isJumpingPressed;


    [Header("Transforms")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private Transform groundCheck;

    [Header("Vector2")]
    private Vector2 InputVectorOnGround;
    private Vector2 InputVectorOnBike;
    private Vector2 InputVectorOnBikeSpeed;
    private Vector2 InputVectorOnJetPack;
    private Vector2 InputVectorOnClimb;

    [Header("Vector3")]
    private Vector3 movement;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;


    private void Awake()
    {
        PMBA = new PlayerMovementBigActionMap();

        playerinput = GetComponent<PlayerInput>();

        moveAnimationID = Animator.StringToHash("Move");
        moveWithBagId = Animator.StringToHash("moveWithBag");
    }

    //Enable Actions
    private void OnEnable()
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
        PMBA.BikeMove.MoveBike.started += MoveOnBike;
        PMBA.BikeMove.MoveBike.canceled += MoveOnBike;
        PMBA.BikeMove.JumpBike.performed += JumpOnBike;
        PMBA.BikeMove.JumpBike.canceled += JumpOnBike;
        PMBA.BikeMove.SpeedBike.started += SpeedOnBike;
        PMBA.BikeMove.SpeedBike.canceled += SpeedOnBike;

        // JetPack ActionMap
        PMBA.JetpackMove.Jetpackmove.started += MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled += MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed += RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled += RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.started += MoveOnClimb;
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
    private void OnDisable()
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
        PMBA.BikeMove.MoveBike.started -= MoveOnBike;
        PMBA.BikeMove.MoveBike.canceled -= MoveOnBike;
        PMBA.BikeMove.JumpBike.performed -= JumpOnBike;
        PMBA.BikeMove.JumpBike.canceled -= JumpOnBike;
        PMBA.BikeMove.SpeedBike.started -= SpeedOnBike;
        PMBA.BikeMove.SpeedBike.canceled -= SpeedOnBike;

        // JetPack ActionMap
        PMBA.JetpackMove.Jetpackmove.started -= MoveOnJetPack;
        PMBA.JetpackMove.Jetpackmove.canceled -= MoveOnJetPack;
        PMBA.JetpackMove.JetPackRise.performed -= RiseOnJetPack;
        PMBA.JetpackMove.JetPackRise.canceled -= RiseOnJetPack;

        // CLimb ActionMap
        PMBA.Climbmove.MoveHandsAndArms.started -= MoveOnClimb;
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



    // Input Methods

    private void MoveOnGround(InputAction.CallbackContext ctx)
    {
        InputVectorOnGround = ctx.ReadValue<Vector2>();


    }

    private void JumpOnGround(InputAction.CallbackContext ctx)
    {

        jump = ctx.ReadValueAsButton();
        
    }

    private void RunOnGround(InputAction.CallbackContext ctx)
    {

        run = true;

    }

    private void MoveOnBike(InputAction.CallbackContext ctx)
    {

        //InputVectorOnBike = ctx.ReadValue<Vector2>();

    }

    private void JumpOnBike(InputAction.CallbackContext ctx)
    {

        jumpOnBike = ctx.ReadValueAsButton();

    }

    private void SpeedOnBike(InputAction.CallbackContext ctx)
    {

        // InputVectorOnBikeSpeed = ctx.ReadValue<Vector2>();

    }

    private void MoveOnJetPack(InputAction.CallbackContext ctx)
    {

        // InputVectorOnJetPack = ctx.ReadValue<Vector2>();
    }

    private void RiseOnJetPack(InputAction.CallbackContext ctx)
    {

        jetPackRise = ctx.ReadValueAsButton();

    }

    private void MoveOnClimb(InputAction.CallbackContext ctx)
    {

        //InputVectorOnClimb = ctx.ReadValue<Vector2>();

    }

    private void RightHand(InputAction.CallbackContext ctx)
    {

        climbRightHand = ctx.ReadValueAsButton();

    }

    private void LeftHand(InputAction.CallbackContext ctx)
    {

        climbLeftHand = ctx.ReadValueAsButton();

    }

    private void RightFoot(InputAction.CallbackContext ctx)
    {

        climbRightFoot = ctx.ReadValueAsButton();

    }

    private void LeftFoot(InputAction.CallbackContext ctx)
    {

        climbLeftFoot = ctx.ReadValueAsButton();

    }

    private void FixedUpdate()
    {
        BasicMovement();

        if (GM.hasBag)
        {
            HandleJumping();
        }
        
       
        
        if (IsGrounded())
        {
            onground = true;
        }
        else
        {
            onground = false;
            playerAnim.SetBool("Jump", false);
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.5f, groundLayer);
    }
    bool OnSlope()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out slopeHit, 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    Vector3 getslope()
    {
        return Vector3.ProjectOnPlane(movement, slopeHit.normal).normalized;
    }

    void BasicMovement()
    {


        if (InputVectorOnGround.magnitude >= .1f && InputVectorOnGround.magnitude <= .5f)
        {
            movement = new Vector3(InputVectorOnGround.x, 0f, InputVectorOnGround.y);
            Vector3 CameraDir = orientation.transform.forward;
            movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
            movement = CameraDir * movement.z + cameraPlayer.transform.right * movement.x;
            Quaternion targetRotation = transform.rotation;
            PlayerRb.MovePosition((Vector3)transform.position + movement * onGroundMoveForceSlow * Time.deltaTime);
            transform.rotation = targetRotation;


            playerAnim.SetFloat(moveAnimationID, .3f, .1f, Time.deltaTime);
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, targetAngle, 0);

            if (!GM.hasBag)
            {
                playerAnim.SetFloat(moveAnimationID, .3f, .1f, Time.deltaTime);
            }
            else if (GM.hasBag)
            {
                playerAnim.SetFloat(moveWithBagId, .3f, .1f, Time.deltaTime);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (run)
            {
                movement = new Vector3(InputVectorOnGround.x, 0f, InputVectorOnGround.y);
                CameraDir = orientation.transform.forward;
                movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
                movement = CameraDir * movement.z + cameraPlayer.transform.right * movement.x;
                targetRotation = transform.rotation;
                PlayerRb.MovePosition((Vector3)transform.position + movement * OnGroundRun * Time.deltaTime);
                transform.rotation = targetRotation;


                playerAnim.SetFloat(moveAnimationID, 1f, .1f, Time.deltaTime);
                targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, targetAngle, 0);

                if (!GM.hasBag)
                {
                    playerAnim.SetFloat(moveAnimationID, 1f, .1f, Time.deltaTime);
                }
                else if (GM.hasBag)
                {
                    playerAnim.SetFloat(moveWithBagId, 1f, .1f, Time.deltaTime);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

        }
        else if (InputVectorOnGround.magnitude >= .5f)
        {
            movement = new Vector3(InputVectorOnGround.x, 0f, InputVectorOnGround.y);
            Vector3 CameraDir = orientation.transform.forward;
            movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
            movement = CameraDir * movement.z + cameraPlayer.transform.right * movement.x;
            Quaternion targetRotation = transform.rotation;
            PlayerRb.MovePosition((Vector3)transform.position + movement * onGroundMoveForceNormal * Time.deltaTime);
            transform.rotation = targetRotation;

            
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, targetAngle, 0);

            if (!GM.hasBag)
            {
                playerAnim.SetFloat(moveAnimationID, .6f, .1f, Time.deltaTime);
            }
            else if (GM.hasBag)
            {
                playerAnim.SetFloat(moveWithBagId, .6f, .1f, Time.deltaTime);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (run)
            {
                movement = new Vector3(InputVectorOnGround.x, 0f, InputVectorOnGround.y);
                CameraDir = orientation.transform.forward;
                movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
                movement = CameraDir * movement.z + cameraPlayer.transform.right * movement.x;
                targetRotation = transform.rotation;
                PlayerRb.MovePosition((Vector3)transform.position + movement * OnGroundRun * Time.deltaTime);
                transform.rotation = targetRotation;


                
                targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, targetAngle, 0);

                if (!GM.hasBag)
                {
                    playerAnim.SetFloat(moveAnimationID, 1f, .1f, Time.deltaTime);
                }
                else if (GM.hasBag)
                {
                    playerAnim.SetFloat(moveWithBagId, 1.5f, .1f, Time.deltaTime);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

        }
        else if (InputVectorOnGround.magnitude <= 0.1f)
        {
            
            if (!GM.hasBag)
            {
                playerAnim.SetFloat(moveAnimationID, 0f, .3f, Time.deltaTime);
            }
            else if (GM.hasBag)
            {
                playerAnim.SetFloat(moveWithBagId, 0f, .3f, Time.deltaTime);
            }
            run = false;
        }
        if (OnSlope())
        {
            PlayerRb.useGravity = false;

            if (PlayerRb.velocity.y > 0)
            {
                PlayerRb.MovePosition(getslope() + movement * onGroundMoveForceNormal * Time.deltaTime);
                PlayerRb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
            else if (PlayerRb.velocity.y < 0)
            {
                PlayerRb.MovePosition(getslope() + movement * onGroundMoveForceNormal * Time.deltaTime);
                PlayerRb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }
        else if (!OnSlope())
        {
            PlayerRb.useGravity = true;
        }
    }

    void HandleJumping()
    {
        // Check if the jump button is pressed and the Rigidbody is on the ground
        if (jump && IsGrounded())
        {
            playerAnim.SetBool("Jump", true);
            jumpTimer = 0f;  // reset the jump timer
            jumpCount++;
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // apply the initial jump force
            
            
        }
        else if (jump && jumpCount < maxJumps && !IsGrounded())
        {
            jumpTimer = 0f;  // reset the jump timer
            jumpCount++;
            PlayerRb.velocity = new Vector3(PlayerRb.velocity.x, 0f, PlayerRb.velocity.z);
            PlayerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // apply the initial jump force
            playerAnim.SetBool("Isgrounded", false);
        }

        // Increase the jump timer if the jump button is held down and the maximum jump time has not been reached
        if (jump && jumpTimer < maxJumpTime && !IsGrounded())
        {
            jumpTimer += Time.deltaTime;
        }

        // Reduce the jump force as the jump timer goes up
        float jumpMultiplier = 1f - (jumpTimer / maxJumpTime);
        jumpMultiplier = Mathf.Clamp01(jumpMultiplier);  // make sure the jump multiplier is between 0 and 1
        Vector3 jumpVelocity = PlayerRb.velocity;
        jumpVelocity.y *= jumpMultiplier;
        PlayerRb.velocity = jumpVelocity;

        // Increase the falling speed if the Rigidbody's velocity is zero
        if (!IsGrounded())
        {
            jump = false;

            PlayerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }
    
}
