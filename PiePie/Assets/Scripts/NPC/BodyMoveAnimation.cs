using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMoveAnimation : MonoBehaviour
{

    
    Vector3 originalPosition; // the original position of this leg to keep the leg fixed to the ground
    public GameObject moveCube; // the move cube of this leg
    public float legMoveSpeed = 7f; // the speed of the leg
    public float moveDistance = 0.7f; // how far does the cube have to go before the leg has to move to it
    public float moveStoppingDistance = 0.4f; // how close the leg should get to the cube before stopping
    public BodyMoveAnimation oppsiteLeg; //the oppsite leg of this one
    bool isMoving = false; // to tell the oppsite leg if this one is moving or not
    bool moving = false; // for this leg to check if its moving or not

    public AnimationCurve arcCurve; // the curve to use for the arc movement
    public float arcHeight = 0.3f; // the height of the arc
    private float arcTime = 0f; // the current time for the arc movement

    private void Start()
    {
        originalPosition = transform.position; // to fix the leg to the ground when the game first launches 
    }

    private void Update()
    {
        float distanceToMoveCube = Vector3.Distance(transform.position, moveCube.transform.position);// calculate the distance from the leg to the cube
        if ((distanceToMoveCube >= moveDistance && !oppsiteLeg.isItMoving()) || moving) //to check if the distance is far away from the cube or not and if it is move the leg to the cube
        {
            moving = true; // to tell this leg that it didnt get close enough to stop moving
            arcTime += Time.deltaTime * legMoveSpeed; // increment the time for the arc movement
            float arcValue = arcCurve.Evaluate(Mathf.Clamp01(arcTime)); // get the value from the curve
            Vector3 arcPosition = Vector3.Lerp(transform.position, moveCube.transform.position, arcValue); // get the new position along the arc
            arcPosition += Vector3.up * arcHeight * Mathf.Sin(arcValue * Mathf.PI); // add the arc height
            transform.position = arcPosition;
            originalPosition = transform.position; // to change the original position and fix the leg to the ground when not moving
            isMoving = true; // to tell oppsite legs that this one is moving

            if (distanceToMoveCube < moveStoppingDistance) // to check if the leg moved all the way instead of just getting in range of the move cube
            {
                moving = false; // to tell the leg it stopped moving and can focus on the ground instead of the cube
                arcTime = 0f; // reset the time for the arc movement
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition + new Vector3(0f, -0.3f, 0f), Time.deltaTime * legMoveSpeed * 3f); // to move the leg down a bit and make it look like walking instead of sliding
            isMoving = false; //to tell the oppiste leg that this leg is not moving
            arcTime = 0f; // reset the time for the arc movement
        }
    }

    public bool isItMoving() //to be called by the oppiste leg to check if the leg is moving or not
    {
        return isMoving;
    }

}
