using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedAnimation : MonoBehaviour
{
    [SerializeField] private LayerMask Ground;
    [SerializeField] private GameObject _parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(_parent.transform.position, Vector3.down, out hit, Ground))
        {
            transform.position = hit.point;
        }
    }
}
