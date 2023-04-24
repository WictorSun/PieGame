using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachineNPC
{

    [CreateAssetMenu(menuName = "States/NPC/Interact")]
    public class NpcWalking : NpcState<NPCController>
    {
        public override void Init(NPCController parent)
        {
            base.Init(parent);


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