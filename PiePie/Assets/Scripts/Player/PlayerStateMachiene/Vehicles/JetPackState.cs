using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Vehicle/JetPack")]
    public class JetPackState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        GameManager _GM;
        IsFacingWall _IFW;
        Jumping _jump;
        AmIGrounded _AIG;
        // Player Rigidbody
        private Rigidbody _playerRB;

        //collider
        private CapsuleCollider _playercollider;

        //Animation
        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;

        //Transforms
        private Transform _orientation;

        private Transform _thisObject;

        public GameObject _bike;
        //Vectors

        private Vector2 _inputVectorOnJetPack;

        //Floats
        private float _rotationSpeed;
        [SerializeField] private float RunAccelRate;
        [SerializeField] private float RunDecelRate;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxfuel;
        [SerializeField] private float _thrustForce;
        [SerializeField] private float _currentFuel;
        [SerializeField] private float _fallMultiplier;
        [SerializeField] private float _timeChangeV;
        [SerializeField] private float _timeLeft;

        private bool _interact;
        private bool _changeVehicle;
        private bool _rise;

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            _currentFuel = _maxfuel;
            _playerAnim = parent.PlayerAnimator;

            _timeLeft = _timeChangeV;

            _bike = parent.Bike;
            _playerRB = parent.PlayerRB;

            _orientation = parent.Orientation;

            _thisObject = parent.ThisObject;

            
            _rotationSpeed = parent.RotationSpeed;
            _playerAnim.SetBool("OnBike", false);
        }
        
        public override void CaptureInput()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {
            _timeLeft -= Time.deltaTime;
            _playercollider = _parent.PlayerCollider;
            _playercollider.enabled = true;
            _bike.SetActive(false);
            _rise = _parent.IH.Rise;
            //Debug.Log(_inputVectorOnJetPack);
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;
            _inputVectorOnJetPack = _parent.IH.InputVectorOnJetPack;
            _interact = _parent.IH.Jump;
            //getting reference to GameManager and InputVectorOnGround from stateRunner
            _GM = _parent.GM;
            _IFW = _parent._IFW;
            _jump = _parent._jumpingSC;
            _AIG = _parent.AIG;
            _changeVehicle = false;
            _changeVehicle = _parent.IH.SwitchVehicle1;
            if(_inputVectorOnJetPack.magnitude >= 0.1f)
            {
                float forward = _inputVectorOnJetPack.y * _moveSpeed;
                float right = _inputVectorOnJetPack.x * _moveSpeed;
                Vector3 targetSpeed = (!_IFW._isFacingWall() ? _orientation.forward : Vector3.zero) * forward + _orientation.right * right;

                Vector3 velocity = _playerRB.velocity;
                velocity.y = 0;

                Vector3 speedDiff = targetSpeed - velocity;

                float accelRate = (Mathf.Abs(targetSpeed.magnitude) >= .1f && Mathf.Abs(targetSpeed.magnitude) <= .5f) ? RunAccelRate : RunDecelRate;



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
                if (_AIG.IsGrounded())
                {
                    _playerAnim.SetFloat(_moveWithBagID, .6f, .1f, Time.deltaTime);
                }
                else
                {
                    _playerAnim.SetFloat(_moveWithBagID, 0f, .1f, Time.deltaTime);
                }
            }


         

            if (_rise && _currentFuel > 0f)
            {
                _currentFuel -= Time.deltaTime;
                _playerRB.AddForce(_playerRB.transform.up * _thrustForce, ForceMode.Impulse);
            }
            else if(_AIG.IsGrounded() && _currentFuel < _maxfuel)
            {
                _currentFuel = _maxfuel;
            }
            //else
            //{
            //    _currentFuel += Time.deltaTime;
            //}
            if(_currentFuel <= 0f && _AIG.IsGrounded())
            {
                _playerRB.AddForce(Vector3.down * _fallMultiplier, ForceMode.Force);
            }
            if(_inputVectorOnJetPack.magnitude <= 0.01f)
            {
                _playerRB.constraints = RigidbodyConstraints.FreezeRotation;
                _playerAnim.SetFloat(_moveWithBagID, 0f, .1f, Time.deltaTime);
            }

        }
        public override void ChangeState()
        {
            if (_changeVehicle && _timeLeft < 0f)
            {
                _runner.SetState(typeof(IdleState));
            }
        }
        public override void Exit()
        {

        }
    }

}