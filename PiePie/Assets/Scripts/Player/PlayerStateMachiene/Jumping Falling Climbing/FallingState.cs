using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Falling")]
    public class FallingState : State<CharacterCtrl>
    {
        // references to stateRunner and GameManager
        CharacterCtrl _parent;
        GameManager _GM;
        IsFacingWall _IFW;
        AmIGrounded _AIG;

        // Player Rigidbody
        private Rigidbody _playerRB;

        //Animation
        private Animator _playerAnim;

        public float _fallMultiplier = 200f;

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            _playerAnim = parent.PlayerAnimator;
            _playerRB = parent.PlayerRB;

        }
      
        public override void CaptureInput()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {
            _GM = _parent.GM;
            _IFW = _parent._IFW;
            _AIG = _parent.AIG;

            if (_playerRB.velocity.y < 0 && !_AIG.IsGrounded())
            {
                _playerRB.AddForce(Vector3.down * _fallMultiplier, ForceMode.Force);
            }
        }
        public override void ChangeState()
        {
            if (_AIG.IsGrounded())
            {
                _runner.SetState(typeof(IdleState));
            }
        }
        public override void Exit()
        {

        }
    }

}

