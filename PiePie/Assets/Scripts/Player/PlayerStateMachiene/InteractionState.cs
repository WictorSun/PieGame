using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/InteractionState")]
    public class InteractionState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        GameManager _GM;
        IsFacingWall _IFW;
        Jumping _jump;

        //collider
        private CapsuleCollider _playercollider;

        public GameObject _bike;

        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;
        private Vector2 _inputVectorOnGround;

       
        private Rigidbody _playerRB;
       

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            _playerRB = parent.PlayerRB;

           
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
         
            _playerAnim.SetBool("OnBike", false);
            _jump = _parent._jumpingSC;
            _GM = _parent.GM;
            _IFW = _parent._IFW;
           
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;

           
            _bike.SetActive(false);
            _playercollider.enabled = true;

            //.Log(_interact);


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
            if (!_GM._isInDia)
            {
                _runner.SetState(typeof(IdleState));
            }
        }
        public override void Exit()
        {

        }
    }

}