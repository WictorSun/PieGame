using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Idle")]
    public  class IdleState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        GameManager _GM;
        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;
        private Vector2 _inputVectorOnGround;
        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            

            _playerAnim = parent.PlayerAnimator;
           

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
            _GM = _parent.GM;
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
           
            if (!_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveAnimationID, .01f, .1f, Time.deltaTime);
            }
            else if (_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveWithBagID, .01f, .1f, Time.deltaTime);
            }
        }
        public override void ChangeState()
        {
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            if (_inputVectorOnGround.magnitude >= .1f && _inputVectorOnGround.magnitude <= .5f)
            {
                _runner.SetState(typeof(SlowWalkState));
            }
        }
        public override void Exit()
        {

        }
    }

}