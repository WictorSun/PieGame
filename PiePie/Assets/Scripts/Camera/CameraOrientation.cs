using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrientation : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    [SerializeField] private Transform bikeOrientation;
    public Transform player;
    public Transform bike;
    public Transform playerObject;
    public Rigidbody rb;

    public float rotationSpeed;

    private void FixedUpdate()
    {
        Vector3 viewdir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 Bikeviewdir = bike.position - new Vector3(transform.position.x, bike.position.y, transform.position.z);
        orientation.forward = viewdir.normalized;
        bikeOrientation.forward = Bikeviewdir.normalized;
    }

}
