using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Before")]
    public  class BeforeState : State<CharacterCtrl>
    {


        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
           
        }

        public override void CaptureInput()
        {

        }
        public override void Update()
        {
            
        }
        public override void FixedUpdate()
        {
            

        }
        public override void ChangeState()
        {
           _runner.SetState(typeof(StartGameState));
        }
        public override void Exit()
        {

        }
    }

}