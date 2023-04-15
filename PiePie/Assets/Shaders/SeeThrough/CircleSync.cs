using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSync : MonoBehaviour
{

    public static int PosID = Shader.PropertyToID("_PlayerPosition");
    public static int SizeID = Shader.PropertyToID("_Size");
    public Material wallMaterial;
    public Camera cameras;
    public LayerMask Obsticlemask;


    void Update()
    {
        var dir = cameras.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (Physics.Raycast(ray, 3000, Obsticlemask))
        {
            wallMaterial = hit.collider.gameObject.GetComponent<Renderer>().material;
            wallMaterial.SetFloat(SizeID, 1f);
        }
        else
        {
            wallMaterial.SetFloat(SizeID, 0f);
        }
        var view = cameras.WorldToViewportPoint(transform.position);
        wallMaterial.SetVector(PosID, view);
    }
}
