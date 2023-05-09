using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/StartGameState")]
    public class StartGameState : State<CharacterCtrl>
    {
        GameManager _GM;
        CharacterCtrl _parent;
        bool _startPlay;
        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            _startPlay = false;
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
            if (_GM.hasBag)
            {
                _startPlay = true;
            }
        }
        public override void ChangeState()
        {
            if (_startPlay)
            {
                _runner.SetState(typeof(IdleState));
            }
            
        }
        public override void Exit()
        {

        }
    }

}