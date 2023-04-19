using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    public class CharacterCtrl : StateRunner<CharacterCtrl>
    {

        [Header("Reference Other Scripts")]
        public PlayerMovementBigActionMap PMBA;
        [SerializeField]private GameManager _GM;
        public InputHandler IH { get; private set; }

        [Header("GameObjects")]
        [SerializeField] private GameObject playerprefab;
        [SerializeField] private GameObject ClimbingPrefab;

        
        [Header("Components")]
        [SerializeField] public Rigidbody playerRb;
      
        [SerializeField] Animator playerAnim;
        private PlayerInput playerinput;
        [SerializeField] private CapsuleCollider playercollider;

        [Header("Animation")]
        int moveAnimationID;
        int moveWithBagId;

        

        

        [Header("Floats")]
        [SerializeField] private float onGroundMoveForceSlow;
        [SerializeField] private float onGroundMoveForceNormal;
        [SerializeField] private float OnGroundRun;
        [SerializeField] private float _rotationSpeed;


        [Header("Jumping")]
        public float jumpForce = 10f;
        public float maxJumpTime = 1f;
        public float fallMultiplier = 2.5f;
        private float jumpTimer = 0f;
        public int maxJumps = 2;
        public int jumpCount = 0;
        public bool isJumpingPressed;

        [Header("Bike")]
        
       

        [Header("Climbing")]
        [SerializeField] public bool readyToClimb;
        [SerializeField] private float climbspeed;
        [SerializeField] private float wallDistanceOffset;
        [SerializeField] private float sideRaycastOffset;

        [Header("Transforms")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform _thisObject;
        [SerializeField] private Transform _cameraPlayer;
        [SerializeField] private Transform groundCheck;

       

        [Header("Vector3")]
        //private Vector3 movement;
        private Vector3 movementBike;
        private Vector3 upDirection = Vector3.up;

        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask ClimbLayer;

        [Header("Slope Handling")]
        [SerializeField] private float maxSlopeAngle;
        private RaycastHit slopeHit;


        //Bike
        [SerializeField] private TurnBikeWheels _tB;
        [SerializeField] private Transform _bikeOrientation;
        [SerializeField] private GameObject _bike;
        [SerializeField] private GameObject _fork;
        [SerializeField] private float _forkRotationSpeed;
        [SerializeField] private float _bikespeed;
        [SerializeField] private float _rotationSpeedBike;
        [SerializeField] private float _minRotateValueSpeedBike;
        [SerializeField] private float _maxRotateValueSpeedBike;
        [SerializeField] private float _minWheelTurn;
        [SerializeField] private float _maxWheelTurn;
        [SerializeField] private float _minValueSpeedBike;
        [SerializeField] private float _maxValueSpeedBike;

        // Hand And Feet Transforms
        [Header("Transforms")]

        [Header("Trackers Bike")]
        [SerializeField] private Transform _leftHandBike, _rightHandBike, _leftFootBike, _rightFootBike;

        [Header("trackers Hands and Feet")]
        [SerializeField] private Transform _leftHand, _rightHand, _leftFoot, _rightFoot;

        [Header("Trackers Climbing")]
        [SerializeField] private Transform _lefthandClimb, _rightHandClimb, _leftFootClimb, _rightFootClimb;

        // Getters And Setters
        public Animator PlayerAnimator { get { return playerAnim; } set { playerAnim = value; } }
        public CapsuleCollider PlayerCollider { get { return playercollider; } set { playercollider = value; } }
        public Rigidbody PlayerRB { get { return playerRb; } set { playerRb = value; } }
        public TurnBikeWheels TB { get { return _tB; } set { _tB = value; } }
        public GameManager GM { get { return _GM; } }



        // Getters And Setters GameObjects
        public GameObject Bike { get { return _bike; } set { _bike = value; } }
        public GameObject Fork { get { return _fork; } set { _fork = value; } }

        // Getters And Setters Floats
        public float BikeSpeed { get { return _bikespeed; } set { _bikespeed = value; } }
        public float ForkRotationSpeed { get { return _forkRotationSpeed; } set { _forkRotationSpeed = value; } }
        public float RotationSpeedBike { get { return _rotationSpeedBike; } set { _rotationSpeedBike = value; } }
        public float RotationSpeed { get { return _rotationSpeed; } }
        public float MinRotateValueSpeedBike { get { return _minRotateValueSpeedBike; } }
        public float MaxRotateValueSpeedBike { get { return _maxRotateValueSpeedBike; } }
        public float MinWheelTurn { get { return _minWheelTurn; } }
        public float MaxWheelTurn { get { return _maxWheelTurn; } }
        public float MinValueSpeedBike { get { return _minValueSpeedBike; } }
        public float MaxValueSpeedBike { get { return _maxValueSpeedBike; } }
        public float OnGroundMoveForceNormal { get { return onGroundMoveForceNormal; } }
        public float OnGroundMoveForceSlow { get { return onGroundMoveForceSlow; } }
        public float OnGroundRunning { get { return OnGroundRun; } }

        // Getters And Setters Animation
        public int MoveAnimationID { get { return moveAnimationID; } set { moveAnimationID = value; } }
        public int MoveWithBagID { get { return moveWithBagId; } set { moveWithBagId = value; } }
        // Getters And Setters Transforms General
        public Transform CameraPlayer { get { return _cameraPlayer; } }
        public Transform ThisObject { get { return _thisObject; } set { _thisObject = value; } }
        public Transform BikeOrientation { get { return _bikeOrientation; } }
        public Transform Orientation { get { return orientation; } }


        //Getters and Setters Hand And Feet Transforms Bikes;
        public Transform LeftHandBike { get { return _leftHandBike; } }
        public Transform LeftFootBike { get { return _leftFootBike; } }
        public Transform RightHandBike { get { return _rightHandBike; } }
        public Transform RightFootBike { get { return _rightFootBike; } }

        //Getters And Setters Hand And Feet Transforms
        public Transform _LeftHand { get { return _leftHand; } set { _leftHand = value; } }
        public Transform _LeftFoot { get { return _leftFoot; } set { _leftFoot = value; } }
        public Transform _RightHand { get { return _rightHand; } set { _rightHand = value; } }
        public Transform _RightFoot { get { return _rightFoot; } set { _rightHand = value; } }

        protected override void Awake()
        {
            base.Awake();
            IH = GetComponent<InputHandler>();
            moveAnimationID = Animator.StringToHash("Move");
            moveWithBagId = Animator.StringToHash("moveWithBag");

            
        }
        private void Update()
        {
            //Debug.Log(IH.InputVectorOnBike);
           // Debug.Log(IH.InputVectorOnBikeSpeed);
        }
    }
}
