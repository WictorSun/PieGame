using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSpinnerNPC : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _headTargetNPC;
    [SerializeField] private Transform _ogTarget;
    [SerializeField] private float _speed;

    bool _isInTrigger;
    
    // Start is called before the first frame update

    private void Update()
    {
        if (_isInTrigger)
        {

            _headTargetNPC.transform.position = Vector3.Lerp(_headTargetNPC.transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            _headTargetNPC.transform.position = Vector3.Lerp(_headTargetNPC.transform.position, _ogTarget.transform.position, _speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInTrigger = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInTrigger = false;

        }
    }
}
