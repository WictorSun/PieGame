using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBikeWheels : MonoBehaviour
{
   
    public GameObject rearWheel;
    public GameObject frontWheel;
    public GameObject crank;
    public GameObject pedalL;
    public GameObject pedalR;
    public float RotationSpeed;
    public float RotationSpeed2;
    public PlayerMovement PM;
    

    public float crankMultiplier = 2f;

    private void Update()
    {
        RotateMeshes();
    }
    void RotateMeshes()
    {
        RotateObject(crank, 1);
        RotateObject(pedalL, -1);
        RotateObject(pedalR, -1);
        RotateObject(rearWheel, crankMultiplier);
        RotateObject(frontWheel, crankMultiplier);
    }
    void RotateObject(GameObject obj, float multiplier)
    {
        //obj.transform.Rotate(Time.fixedDeltaTime * rb.velocity.magnitude * (360f / RotationSpeed) * multiplier, 0, 0);
        obj.transform.Rotate(Time.deltaTime * PM.InputVectorOnBike.magnitude * (360f / RotationSpeed) * multiplier, 0, 0);
    }
}
