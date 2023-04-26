using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Pause")]
    public class PauseState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        GameManager _GM;
       

       
        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;
        private Rigidbody _playerRB;


        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;

            _playerRB = parent.PlayerRB;
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
           
           
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;


           

            //.Log(_interact);


            if (!_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveAnimationID, 0f, 0.05f, Time.deltaTime);
            }
            else if (_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveWithBagID, 0f, 0.05f, Time.deltaTime);
            }
            //_playerRB.constraints = RigidbodyConstraints.FreezeRotation;



        }
        public override void ChangeState()
        {
            if (!_GM._CamIsActive)
            {
                _runner.SetState(typeof(IdleState));
            }
        }
        public override void Exit()
        {

        }
    }

}