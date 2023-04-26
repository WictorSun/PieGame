
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Normal Walk State")]
    public class NormalWalkState : State<CharacterCtrl>
    {
        // references to stateRunner and GameManager
        CharacterCtrl _parent;
        GameManager _GM;
        IsFacingWall _IFW;
        AmIGrounded _AIG;
        Jumping _jump;
        // Player Rigidbody
        private Rigidbody _playerRB;

        //Animation
        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;

        //Transforms
        private Transform _orientation;
        
        private Transform _thisObject;

        //Vectors
       
        private Vector2 _inputVectorOnGround;

        //Floats
        private float _onGroundMoveForceNormal;
        private float _rotationSpeed;
        [SerializeField] private float RunAccelRate;
        [SerializeField] private float RunDecelRate;
        [SerializeField] private float _moveSpeed;

        private bool _interact;

        private bool _changeVehicle;

        private bool _run;

        public override void Init(CharacterCtrl parent)
        {
            
            base.Init(parent);
            _parent = parent;

            _playerAnim = parent.PlayerAnimator;
            

            _playerRB = parent.PlayerRB;

            _orientation = parent.Orientation;
           
            _thisObject = parent.ThisObject;

            _onGroundMoveForceNormal = parent.OnGroundMoveForceNormal;
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
            _jump = _parent._jumpingSC;
            _run = _parent.IH.Run;
            _AIG = _parent.AIG;
            _interact = _parent.IH.Jump;
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            //getting reference to GameManager and InputVectorOnGround from stateRunner
            _GM = _parent.GM;
            _IFW = _parent._IFW;
            _changeVehicle = _parent.IH.SwitchVehicle1;

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



            //Checks which Animation BlendTree to play
            if (!_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveAnimationID, .6f, .1f, Time.deltaTime);
            }
            else if (_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveWithBagID, .6f, .1f, Time.deltaTime);
            }
            

        }
        public override void ChangeState()
        {
             _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            if (_inputVectorOnGround.magnitude >= .1f && _inputVectorOnGround.magnitude <= .5f)
            {
                _runner.SetState(typeof(SlowWalkState));
            }
            else if (_inputVectorOnGround.magnitude <= 0)
            {
                _runner.SetState(typeof(IdleState));
            }
            if (_GM.hasBag && _IFW._isFacingClimbableWall() && _interact)
            {
                _runner.SetState(typeof(ClimbState));
            }
            //if (!_AIG.IsGrounded() && !_jump._isJumpingNow)
            //{
            //    _runner.SetState(typeof(FallingState));
            //}
            if (_AIG.IsGrounded() && _GM.hasBike && _changeVehicle)
            {
                _runner.SetState(typeof(BikeState));
            }
            if(_AIG.IsGrounded() && _run && _GM.hasBag)
            {

                _runner.SetState(typeof(RunningState));
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
