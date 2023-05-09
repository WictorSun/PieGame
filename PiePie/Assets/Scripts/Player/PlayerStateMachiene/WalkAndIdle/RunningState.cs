using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Running State")]
    public class RunningState : State<CharacterCtrl>
    { 
    // references to stateRunner and GameManager
    CharacterCtrl _parent;
    GameManager _GM;
    IsFacingWall _IFW;
    // Player Rigidbody
    private Rigidbody _playerRB;

    //Animation
    private Animator _playerAnim;
    int _moveAnimationID;
    int _moveWithBagID;

    //Transforms
    private Transform _orientation;
    private Transform _cameraPlayer;
    private Transform _thisObject;

    //Vectors
    private Vector3 movement;
    private Vector2 _inputVectorOnGround;

    //Floats
    private float _OnGroundRun;
    private float _rotationSpeed;
    [SerializeField] private float RunAccelRate;
    [SerializeField] private float RunDecelRate;
    [SerializeField] private float _moveSpeed;


        //Bools
        private bool _run;

        public override void Init(CharacterCtrl parent)
    {

        base.Init(parent);
        _parent = parent;

        _playerAnim = parent.PlayerAnimator;


        _playerRB = parent.PlayerRB;

        _orientation = parent.Orientation;
        _cameraPlayer = parent.CameraPlayer;
        _thisObject = parent.ThisObject;

        _OnGroundRun = parent.OnGroundRunning;
        _rotationSpeed = parent.RotationSpeed;
        _playerAnim.SetBool("OnBike", false);
    }



    //Checks Input
    public override void CaptureInput()
    {

    }
    public override void Update()
    {

    }
    // Update When Using Physics
    public override void FixedUpdate()
    {
            _run = _parent.IH.Run;
            _run = false;
            _IFW = _parent._IFW;
            _moveAnimationID = _parent.MoveAnimationID;
        _moveWithBagID = _parent.MoveWithBagID;
        _inputVectorOnGround = _parent.IH.InputVectorOnGround;
        //getting reference to GameManager and InputVectorOnGround from stateRunner
        _GM = _parent.GM;
            

            float forward = _inputVectorOnGround.y * _moveSpeed;
            float right = _inputVectorOnGround.x * _moveSpeed;
            Vector3 targetSpeed = (!_IFW._isFacingWall() ? _orientation.forward : Vector3.zero) * forward + _orientation.right * right;

            Vector3 velocity = _playerRB.velocity;
            velocity.y = 0;

            Vector3 speedDiff = targetSpeed - velocity;

            float accelRate = (Mathf.Abs(targetSpeed.magnitude) >= 0.5f) ? RunAccelRate : RunDecelRate;



            Vector3 movement = speedDiff * accelRate;

            //if (_normalVector != Vector3.zero)
            //    movement = Vector3.ProjectOnPlane(movement, _normalVector);

            _playerRB.AddForce(movement, ForceMode.Force);



            ////Rotating the player
            Quaternion targetRotation = _thisObject.transform.rotation;
            _thisObject.transform.rotation = targetRotation;
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, targetAngle, 0);
            _thisObject.transform.rotation = Quaternion.Lerp(_thisObject.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);



            ////Moving the player
            //movement = new Vector3(_inputVectorOnGround.x, 0f, _inputVectorOnGround.y);
            //Vector3 CameraDir = _orientation.transform.forward;
            //movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
            //movement = CameraDir * movement.z + _cameraPlayer.transform.right * movement.x;
            //_playerRB.MovePosition((Vector3)_thisObject.transform.position + movement * _OnGroundRun * Time.deltaTime);

            ////Rotating the player
            //Quaternion targetRotation = _thisObject.transform.rotation;
            //_thisObject.transform.rotation = targetRotation;
            //float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            //targetRotation = Quaternion.Euler(0, targetAngle, 0);
            //_thisObject.transform.rotation = Quaternion.Lerp(_thisObject.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            //Checks which Animation BlendTree to play
            if (!_GM.hasBag)
            {
            _playerAnim.SetFloat(_moveAnimationID, 1.2f, .1f, Time.deltaTime);
            }
        else if (_GM.hasBag)
            {
            _playerAnim.SetFloat(_moveWithBagID, 1.2f, .1f, Time.deltaTime);
            }
            Debug.Log(_inputVectorOnGround);

    }
    public override void ChangeState()
    {
            if (_GM.hasBag && _IFW._isFacingClimbableWall() && _parent.IH.Interact)
            {
                _runner.SetState(typeof(ClimbState));
            }
            if(_inputVectorOnGround.magnitude >= 0.1f && _inputVectorOnGround.magnitude <= 0.5f)
            {
                _runner.SetState(typeof(SlowWalkState));
            }
            if(_inputVectorOnGround.magnitude <= 0.1f)
            {
                _runner.SetState(typeof(IdleState));
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
