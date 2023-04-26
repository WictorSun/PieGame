using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Idle")]
    public  class IdleState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        [SerializeField] GameManager _GM;
        IsFacingWall _IFW;
        Jumping _jump;

        //collider
        private CapsuleCollider _playercollider;

        public GameObject _bike;

        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;
        private Vector2 _inputVectorOnGround;

        //Jumping
        AmIGrounded _AIG;
        private Rigidbody _playerRB;
        //Bools
        private bool _isGrounded;
        private bool _changeVehicle;

        //Floats
        [SerializeField] private float _timeChangeV;
        [SerializeField] private float _timeLeft;

        private bool _interact;

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            _playerRB = parent.PlayerRB;
            _GM = _parent.GM;
            _timeLeft = _timeChangeV;
            //_changeVehicle = false;

            _playerAnim = parent.PlayerAnimator;
            _playercollider = parent.PlayerCollider;
            _bike = parent.Bike;
        }

        public override void CaptureInput()
        {

        }
        public override void Update()
        {
            
        }
        public override void FixedUpdate()
        {
            _AIG = _parent.AIG;
            _playerAnim.SetBool("OnBike", false);
            _jump = _parent._jumpingSC;
            _GM = _parent.GM;
            _IFW = _parent._IFW;
            _interact = _parent.IH.Jump;
            _changeVehicle = _parent.IH.SwitchVehicle1;
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;

            _timeLeft -= Time.deltaTime;

            _bike.SetActive(false);
            _playercollider.enabled = true;

                
                if (!_GM.hasBag)
                {
                    _playerAnim.SetFloat(_moveAnimationID, 0f, 0.05f, Time.deltaTime);
                }
                else if (_GM.hasBag)
                {
                    _playerAnim.SetFloat(_moveWithBagID, 0f, 0.05f, Time.deltaTime);
                }
            _playerRB.constraints = RigidbodyConstraints.FreezeRotation;
            


        }
        public override void ChangeState()
        {
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            if (_inputVectorOnGround.magnitude >= .1f && _inputVectorOnGround.magnitude <= .5f)
            {
                _runner.SetState(typeof(SlowWalkState));
            }
            if(_inputVectorOnGround.magnitude >= 0.5f)
            {
                _runner.SetState(typeof(NormalWalkState));
            }
            if (_GM.hasBag && _IFW._isFacingClimbableWall() && _interact)
            {
                _runner.SetState(typeof(ClimbState));
            }
            //if (!_AIG.IsGrounded() && !_jump._isJumpingNow)
            //{
            //    _runner.SetState(typeof(FallingState));
            //}
            if (_GM.hasBike && _changeVehicle && _timeLeft < 0f)
            {

                _runner.SetState(typeof(BikeState));
            }
            if (_GM._isInDia)
            {
                _runner.SetState(typeof(InteractionState));
                
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