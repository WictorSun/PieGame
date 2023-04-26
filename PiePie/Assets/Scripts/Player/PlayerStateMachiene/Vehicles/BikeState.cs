using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Vehicle/Bike")]
    public class BikeState : State<CharacterCtrl>
    {

        
        CharacterCtrl _parent;
        IsFacingWall _IFW;
        AmIGrounded _AIG;
        GameManager _GM;

        //collider
        private CapsuleCollider _playercollider;

        
        // Animator
        private Animator _playerAnim;

        //Generic Transforms
        private Transform _bikeOrientation;
        private Transform _cameraPlayer;
        private Transform _thisObject;

        //Rigidbody off the player
        private Rigidbody _playerRB;

        //Wheel script
        private TurnBikeWheels _TB;

        // GameObjects
        public GameObject _bike;
        public GameObject _fork;

        // Transforms on bike for hands and feet
        private Transform _leftFootBike;
        private Transform _rightFootBike;
        private Transform _leftHandBike;
        private Transform _rightHandBike;

        //Transforms on player Hands And Feet

        private Transform _leftHand;
        private Transform _leftFoot;
        private Transform _rightHand;
        private Transform _rightFoot;

        //Vectors
        private Vector2 _inputVectorOnBike;
        private Vector2 _inputVectorOnBikeSpeed;
        private Vector2 _bikeMove;
        private Vector3 _movementBike;
        private Vector2 previousJoystickValue;

        // Floats
        private float _bikeSpeed;
        private float _forkRotationSpeed;
        private float _rotationSpeedBike;
        private float _rotationSpeed;
        private float _minRotateValueSpeedBike;
        private float _maxRotateValueSpeedBike;
        private float _minWheelTurn;
        private float _maxWheelTurn;
        private float _minValueSpeedBike;
        private float _maxValueSpeedBike;
        [SerializeField] private float RunAccelRate;
        [SerializeField] private float RunDecelRate;
        [SerializeField] private float _timeChangeV;
        [SerializeField] private float _timeLeft;

        //Bools
        private bool _changeVehicle;


        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);

            _parent = parent;
            _playercollider = parent.PlayerCollider;
            _playerAnim = parent.PlayerAnimator;
            _playerRB = parent.PlayerRB;
            _TB = parent.TB;

            _bikeOrientation = parent.BikeOrientation;
            _cameraPlayer = parent.CameraPlayer;
            _thisObject = parent.ThisObject;

            _bike = parent.Bike;
            _fork = parent.Fork;

            _leftFootBike = parent.LeftFootBike;
            _rightFootBike = parent.RightFootBike;
            _leftHandBike = parent.LeftHandBike;
            _rightHandBike = parent.RightHandBike;

            _leftFoot = parent._LeftFoot;
            _rightFoot = parent._RightFoot;
            _leftHand = parent._LeftHand;
            _rightHand = parent._RightHand;
            //_changeVehicle = false;
            _timeLeft = _timeChangeV;

            _bikeSpeed = parent.BikeSpeed;
            _forkRotationSpeed = parent.ForkRotationSpeed;
            _rotationSpeedBike = parent.RotationSpeedBike;
            _rotationSpeed = parent.RotationSpeed;
            _minRotateValueSpeedBike = parent.MinRotateValueSpeedBike;
            _maxRotateValueSpeedBike = parent.MaxRotateValueSpeedBike;
            _minWheelTurn = parent.MinWheelTurn;
            _maxWheelTurn = parent.MaxWheelTurn;
            _minValueSpeedBike = parent.MinValueSpeedBike;
            _maxValueSpeedBike = parent.MaxValueSpeedBike;

           
        }
        
        public override void CaptureInput()
        {
            
        }
        public override void Update()
        {
          
        }
        public override void FixedUpdate()
        {
            Debug.Log(_changeVehicle);
            _changeVehicle = _parent.IH.SwitchVehicle1;
            // setting the vector of the movement
            _inputVectorOnBike = _parent.IH.InputVectorOnBike;

            _inputVectorOnBikeSpeed = _parent.IH.InputVectorOnBikeSpeed;

            _timeLeft -= Time.deltaTime;

            _AIG = _parent.AIG;

            _GM = _parent.GM;
           
            //Setting the animation for the bike state
            _playerAnim.SetBool("OnBike", true);

            // Assigning different body parts off the player to the bike
            _leftFoot.transform.position = _leftFootBike.transform.position;      
            _rightFoot.transform.position = _rightFootBike.transform.position;    
            _leftHand.transform.position = _leftHandBike.transform.position;      
            _rightHand.transform.position = _rightHandBike.transform.position;    

            // turning on the Bike GameObject
            _bike.SetActive(true);

            // Turning off the Collider on the Player
            _playercollider.enabled = false;

            //Bike Moves
            if (_inputVectorOnBike.magnitude >= .1f)
            {
                _IFW = _parent._IFW;


                float forward = _inputVectorOnBike.y * _bikeSpeed;
                float right = _inputVectorOnBike.x * _bikeSpeed;
                Vector3 targetSpeed = (!_IFW._isFacingWall() ? _bikeOrientation.forward : Vector3.zero) * forward + _bikeOrientation.right * right;

                Vector3 velocity = _playerRB.velocity;
                velocity.y = 0;

                Vector3 speedDiff = targetSpeed - velocity;

                float accelRate = (Mathf.Abs(targetSpeed.magnitude) >= 0.5f) ? RunAccelRate : RunDecelRate;



                Vector3 movement = speedDiff * accelRate;

                //if (_normalVector != Vector3.zero)
                //    movement = Vector3.ProjectOnPlane(movement, _normalVector);

                _playerRB.AddForce(movement, ForceMode.Force);  

                //Rotates bike
                Quaternion targetRotation = _thisObject.transform.rotation;
                _thisObject.transform.rotation = targetRotation;
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, targetAngle, 0);
                _thisObject.transform.rotation = Quaternion.Slerp(_thisObject.transform.rotation, targetRotation, _rotationSpeedBike * Time.deltaTime);

                //Rotates fork of bike
                float forkTargetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                Quaternion forkTargetRotation = Quaternion.Euler(-90, forkTargetAngle - 180, 0);
                _fork.transform.rotation = Quaternion.Slerp(_fork.transform.rotation, forkTargetRotation, _forkRotationSpeed * Time.deltaTime);

            }

            //controlls speed of bike and rotations on pedals and wheels
            Vector2 joystickvalue = _inputVectorOnBikeSpeed;

            // Calculate the rotation angle based on input
            float rotationAngle = (joystickvalue.x + previousJoystickValue.x) * _rotationSpeed * Time.deltaTime;
            float rotationAngleBike = (joystickvalue.x + previousJoystickValue.x) * _rotationSpeed * Time.deltaTime;
            float rotationAngleTurnWheels = (joystickvalue.x + previousJoystickValue.x) * _rotationSpeed * Time.deltaTime;



            // Update the current value based on the rotation angle
            _bikeSpeed += rotationAngle;
            _rotationSpeedBike += rotationAngleBike;
            _TB.RotationSpeed += rotationAngleTurnWheels;

            // Clamp the current value within the specified range
            _bikeSpeed = Mathf.Clamp(_bikeSpeed, _minValueSpeedBike, _maxValueSpeedBike);
            _rotationSpeedBike = Mathf.Clamp(_rotationSpeedBike, _minRotateValueSpeedBike, _maxRotateValueSpeedBike);
            _TB.RotationSpeed = Mathf.Clamp(_TB.RotationSpeed, _minWheelTurn, _maxWheelTurn);

            // Update the previous joystick value for the next frame
            previousJoystickValue = joystickvalue;
        }
        public override void ChangeState()
        {
            if (_changeVehicle && !_GM._hasJetPack && _timeLeft <= 0f)
            {
                _runner.SetState(typeof(IdleState));
            }

            if (_changeVehicle && _GM._hasJetPack && _timeLeft <= 0f)
            {
                _runner.SetState(typeof(JetPackState));
            }
            if (_GM._CamIsActive)
            {
                _runner.SetState(typeof(PauseState));
            }
        }
        public override void Exit()
        {
           
        }
        
    }

}