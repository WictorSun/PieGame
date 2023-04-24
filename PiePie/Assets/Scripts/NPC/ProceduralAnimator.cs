using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    public LayerMask _groundLayer;
    public ProceduralAnimator _PA;
    public float _stepDistance;
    public float _stepHeight;
    public float _stepLenght;
    public float _footSpacing;
    public float _speed;
    public Transform body;
    public Vector3 footOffset;

    Vector3 _oldposition;
    Vector3 _newPos;
    Vector3 _currPos;
    Vector3 _oldNorm;
    Vector3 _currNorm;
    Vector3 _newNorm;

    float lerp;

    private void Start()
    {
        _footSpacing = transform.localPosition.x;
        _oldposition = _newPos = _currPos = transform.position;
        _oldNorm = _currNorm = _newNorm = transform.up;
        lerp = 1;
    }

    private void Update()
    {
        transform.position = _currPos;
        transform.up = _currNorm;

        Ray ray = new Ray(body.position + (body.forward * _footSpacing), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1, _groundLayer.value))
        {
            if(Vector3.Distance(_newPos, hit.point) > _stepDistance && !_PA.isMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(_newPos).z ? 1 : -1;
                _newPos = hit.point + (body.forward * _stepLenght * direction) + footOffset;
                _newNorm = hit.normal;
            }
        }
        if(lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(_oldposition, _newPos, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * _stepHeight;
            _currPos = tempPos;
            _currNorm = Vector3.Lerp(_oldNorm, _newNorm, lerp);
            lerp += Time.deltaTime * _speed;

        }
        else
        {
            _oldposition = _newPos;
            _oldNorm = _newNorm;
        }
    }

    public bool isMoving()
    {
        return lerp < 1;
    }

}