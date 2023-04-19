using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Jumping")]
    public class JumpingState : State<CharacterCtrl>
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

        }
        public override void Exit()
        {

        }
    }

}