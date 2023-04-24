using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmIGrounded : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _hasClimbedWallLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private bool _grounded;

    
    public bool Grounded { get { return _grounded; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, 0.5f, _groundLayer);
    }
    public bool IsGroundedOnClimb()
    {
        return Physics.CheckSphere(_groundCheck.position, 0.5f, _hasClimbedWallLayer);
    }
}
