using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFacingWall : MonoBehaviour
{
    [SerializeField] private LayerMask _WallLayer;
    [SerializeField] private Transform _wallCheck;

    [SerializeField] private LayerMask _NPC;
    [SerializeField] private LayerMask _climbLayer;
    [SerializeField] private Animator _interactA;
    [SerializeField] private InputHandler _IH;
    [SerializeField] private bool _climb;

    private void Update()
    {
        if (_IH.Interact)
        {
            _climb = true;
        }
        if (_IH.CanleClimbing)
        {
            _climb = false;
        }
        if (_isFacingClimbableWall() && !_climb)
        {
            _interactA.SetBool("Play", true);
        }
        if (_climb || !_isFacingClimbableWall())
        {
            _interactA.SetBool("Play", false);
        }
    }

    public bool _isFacingWall()
    {
        return Physics.CheckSphere(_wallCheck.position, 0.5f, _WallLayer);
    }
    public bool _isFacingClimbableWall()
    {
        return Physics.CheckSphere(_wallCheck.position, 1f, _climbLayer);
    }
   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_wallCheck.position, 1f);
    }
}
