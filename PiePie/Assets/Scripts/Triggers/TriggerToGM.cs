using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToGM : MonoBehaviour
{
    [SerializeField] private string objectInColliederString;
    
    [SerializeField] private GameManager GM;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(objectInColliederString))
        {
            GM.isInClimbingArea = true;
        }
    }
    //public void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag(objectInColliederString))
    //    {
    //        GM.isInClimbingArea = true;
    //    }
    //}
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(objectInColliederString))
        {
            GM.isInClimbingArea = false;
        }
    }
}
