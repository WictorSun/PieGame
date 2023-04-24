using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachineNPC
{
    public class NPCController : NPCStateRunner<NPCController>
    {
        [Header("Reference Other Scripts")]
       
        [SerializeField] private GameManager _GM;
       
   
        public GameManager GM { get { return _GM; } set { _GM = value; } }
        

        protected override void Awake()
        {
            base.Awake();


        }
    }
}