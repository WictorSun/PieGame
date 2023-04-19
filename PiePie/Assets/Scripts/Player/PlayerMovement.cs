using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Windows;
using static UnityEngine.LightAnchor;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference Other Scripts")]
    PlayerMovementBigActionMap PMBA;
    [SerializeField] private GameManager GM;
    [SerializeField] private TurnBikeWheels TB;

    [Header("GameObjects")]
    [SerializeField] private GameObject playerprefab;
    [SerializeField] private GameObject ClimbingPrefab;

    [Header("Components")]
    [SerializeField] public Rigidbody PlayerRb;
    //[SerializeField] Rigidbody BikeRb;
    [SerializeField] Animator playerAnim;
    private PlayerInput playerinput;
    [SerializeField] private CapsuleCollider playercollider;

    [Header("Animation")]
    int moveAnimationID;
    int moveWithBagId;

    [Header("Transforms")]

    [Header("Trackers Bike")]
    [SerializeField] private Transform LefthandBike, rightHandBike, leftFootBike, rightFootBike;

    [Header("trackers Hands and Feet")]
    [SerializeField] private Transform Lefthand, rightHand, leftFoot, rightFoot;

    [Header("Trackers Climbing")]
    [SerializeField] private Transform LefthandClimb, rightHandClimb, leftFootClimb, rightFootClimb;

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


    [Header("Floats")]
    [SerializeField] private float onGroundMoveForceSlow;
    [SerializeField] private float onGroundMoveForceNormal;
    [SerializeField] private float OnGroundRun;
    [SerializeField] private float rotationSpeed;


    [Header("Jumping")]
    public float jumpForce = 10f;
    public float maxJumpTime = 1f;
    public float fallMultiplier = 2.5f;
    public float jumpTimer = 0f;
    public int maxJumps = 2;
    public int jumpCount = 0;
    public bool isJumpingPressed;

    [Header("Bike")]
    [SerializeField] private float normalBikeSpeed;
    [SerializeField] private GameObject bike;
    [SerializeField] private GameObject fork;
    private float rotationValue = 0f;
    [SerializeField] private float forkRotationSpeed;
    [SerializeField] private float rotationSpeedBike;
    [SerializeField] private float minRotateValueSpeedBike;
    [SerializeField] private float maxRotateValueSpeedBike;
    [SerializeField] private Transform seat;
    [SerializeField] private float bikespeed;
    [SerializeField] private float minWheelTurn;
    [SerializeField] private float maxWheelTurn;
    [SerializeField] private float SpeedRotationJoystickBike;
    [SerializeField] private float minValueSpeedBike;
    [SerializeField] private float maxValueSpeedBike;
    private Vector2 previousJoystickValue;

    [Header("Climbing")]
    [SerializeField] public bool readyToClimb;
    [SerializeField] private float climbspeed;
    [SerializeField] private float wallDistanceOffset;
    [SerializeField] private float sideRaycastOffset;

    [Header("Transforms")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform bikeOrientation;
    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private Transform groundCheck;

    [Header("Vector2")]
    private Vector2 InputVectorOnGround;
    public Vector2 InputVectorOnBike;
    private Vector2 InputVectorOnBikeSpeed;
    private Vector2 InputVectorOnJetPack;
    private Vector2 InputVectorOnClimb;

    [Header("Vector3")]
    private Vector3 movement;
    private Vector3 movementBike;
    private Vector3 upDirection = Vector3.up;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ClimbLayer;

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

        InputVectorOnBike = ctx.ReadValue<Vector2>();

    }

    private void JumpOnBike(InputAction.CallbackContext ctx)
    {

        jumpOnBike = ctx.ReadValueAsButton();

    }

    private void SpeedOnBike(InputAction.CallbackContext ctx)
    {

        InputVectorOnBikeSpeed = ctx.ReadValue<Vector2>();

    }
    private void BreakOnBike(InputAction.CallbackContext ctx)
    {
        breakOnBike = ctx.ReadValueAsButton();
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

        InputVectorOnClimb = ctx.ReadValue<Vector2>();

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


        if (GM.hasBag && !GM.isReadyToClimb) // checks if the player has a bag
        {
            HandleJumping();
        }

        if (bike.active && GM.hasBag && GM.hasBike) // checks if bike is selected and purcased
        {
            BikeMovement();

        }
        if (!bike.active && !GM.isReadyToClimb)
        {

            BasicMovement();
            playerAnim.SetBool("OnBike", false);
            bike.SetActive(false);
            playercollider.enabled = true;
            ClimbingPrefab.SetActive(false);
        }

        if (GM.isReadyToClimb)
        {
            readyToClimb = true;
            ClimbingMovement();
        }
        else
        {
            ClimbingPrefab.SetActive(false);
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

    void BikeMovement()
    {


        playerAnim.SetBool("OnBike", true);                                 // Switches Blend Tree
        leftFoot.transform.position = leftFootBike.transform.position;      //trackers for procedrual animations moves transfor to the ones theire supposed to be moving with
        rightFoot.transform.position = rightFootBike.transform.position;    //trackers for procedrual animations moves transfor to the ones theire supposed to be moving with
        Lefthand.transform.position = LefthandBike.transform.position;      //trackers for procedrual animations moves transfor to the ones theire supposed to be moving with
        rightHand.transform.position = rightHandBike.transform.position;    //trackers for procedrual animations moves transfor to the ones theire supposed to be moving with
                                                                            //bike.transform.SetParent(this.transform);                               





        //BikeMovement
        if (InputVectorOnBike.magnitude >= .1f)
        {
            playercollider.enabled = false;

            //Bike Moves
            movementBike = new Vector3(InputVectorOnBike.x, 0f, InputVectorOnBike.y);
            Vector3 CameraDir = bikeOrientation.transform.forward;
            movementBike = Vector3.Scale(movementBike, new Vector3(1, 0, 1)).normalized;
            movementBike = CameraDir * movementBike.z + cameraPlayer.transform.right * movementBike.x;
            Quaternion targetRotation = transform.rotation;
            PlayerRb.MovePosition((Vector3)transform.position + movementBike * bikespeed * Time.deltaTime);
            transform.rotation = targetRotation;

            //Rotates bike 
            float targetAngle = Mathf.Atan2(movementBike.x, movementBike.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeedBike * Time.deltaTime);

            //Rotates fork of bike
            float forkTargetAngle = Mathf.Atan2(movementBike.x, movementBike.z) * Mathf.Rad2Deg;
            Quaternion forkTargetRotation = Quaternion.Euler(-90, forkTargetAngle - 180, 0);
            fork.transform.rotation = Quaternion.Slerp(fork.transform.rotation, forkTargetRotation, forkRotationSpeed * Time.deltaTime);







        }

        //controlls speed of bike and rotations on pedals and wheels
        Vector2 joystickvalue = InputVectorOnBikeSpeed;

        // Calculate the rotation angle based on input
        float rotationAngle = (joystickvalue.x + previousJoystickValue.x) * rotationSpeed * Time.deltaTime;
        float rotationAngleBike = (joystickvalue.x + previousJoystickValue.x) * rotationSpeed * Time.deltaTime;
        float rotationAngleTurnWheels = (joystickvalue.x + previousJoystickValue.x) * rotationSpeed * Time.deltaTime;



        // Update the current value based on the rotation angle
        bikespeed += rotationAngle;
        rotationSpeedBike += rotationAngleBike;
        TB.RotationSpeed += rotationAngleTurnWheels;

        // Clamp the current value within the specified range
        bikespeed = Mathf.Clamp(bikespeed, minValueSpeedBike, maxValueSpeedBike);
        rotationSpeedBike = Mathf.Clamp(rotationSpeedBike, minRotateValueSpeedBike, maxRotateValueSpeedBike);
        TB.RotationSpeed = Mathf.Clamp(TB.RotationSpeed, minWheelTurn, maxWheelTurn);

        // Update the previous joystick value for the next frame
        previousJoystickValue = joystickvalue;

    }

    void ClimbingMovement()
    {
        ClimbingPrefab.SetActive(true);

        playerAnim.SetBool("IsClimbing", true);

        float h = InputVectorOnClimb.x;
        float v = InputVectorOnClimb.y;
        Vector2 input = SquareToCircle(new Vector2(h, v));

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20, ClimbLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green); // Draw the forward raycast in green
            transform.forward = -hit.normal;
            float targetDistance = hit.distance - wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
            PlayerRb.position = Vector3.Lerp(PlayerRb.position, hit.point + hit.normal * targetDistance, 30f * Time.fixedDeltaTime);
        }
        else
        {
            GM.isReadyToClimb = false;
        }

        // Draw side raycasts to visualize wall detection
        Vector3 leftRaycastOrigin = transform.position + transform.right * -sideRaycastOffset;
        Vector3 rightRaycastOrigin = transform.position + transform.right * sideRaycastOffset;
        RaycastHit leftHit;
        RaycastHit rightHit;
        if (Physics.Raycast(leftRaycastOrigin, -transform.up, out leftHit, 0.3f, ClimbLayer) && Physics.Raycast(rightRaycastOrigin, -transform.up, out rightHit, 0.3f, ClimbLayer))
        {
            Debug.DrawRay(leftRaycastOrigin, -transform.up * leftHit.distance, Color.blue); // Draw the left side raycast in blue
            Debug.DrawRay(rightRaycastOrigin, -transform.up * rightHit.distance, Color.red); // Draw the right side raycast in red

            // Determine which wall to climb based on the side raycast hits
            if (leftHit.distance < rightHit.distance)
            {
                // Climb on the left wall
                float targetDistance = leftHit.distance - wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
                PlayerRb.position = Vector3.Lerp(PlayerRb.position, leftHit.point + leftHit.normal * targetDistance, 10f * Time.fixedDeltaTime);
            }
            else
            {
                // Climb on the right wall
                float targetDistance = rightHit.distance - wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
                PlayerRb.position = Vector3.Lerp(PlayerRb.position, rightHit.point + rightHit.normal * targetDistance, 10f * Time.fixedDeltaTime);
            }
        }

        PlayerRb.velocity = transform.TransformDirection(input) * climbspeed;
    }

    Vector2 SquareToCircle(Vector2 input)
    {

        return (input.sqrMagnitude >= 1f) ? input.normalized : input;
    }

}