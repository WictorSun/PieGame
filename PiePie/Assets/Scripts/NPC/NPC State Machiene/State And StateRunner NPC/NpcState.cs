using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachineNPC
{


    public abstract class NpcState<T> : ScriptableObject where T : MonoBehaviour
    {
        protected T _runner;

        public virtual void Init(T parent)
        {
            _runner = parent;
        }
        
      
        public abstract void Update(); 
        public abstract void FixedUpdate();
        public abstract void ChangeState();
        public abstract void Exit();
    }

}
