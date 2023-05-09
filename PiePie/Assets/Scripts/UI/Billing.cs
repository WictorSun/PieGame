using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billing : MonoBehaviour
{
    Vector3 _cameraDir;
    [SerializeField] private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cameraDir = _cam.transform.forward;
        

        transform.rotation = Quaternion.LookRotation(_cameraDir);
    }
}
