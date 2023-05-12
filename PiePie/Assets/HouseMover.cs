using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMover : MonoBehaviour
{
    public bool _houseInTheWay;
    public GameObject _house;
    public float _speed;
    private float _downSpeed = 2;
    public Transform _startpoint;
    public Transform _endpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_houseInTheWay)
        {
            _house.transform.position = Vector3.Lerp(_house.transform.position, _endpoint.transform.position, _speed * Time.deltaTime);
        }
        else if (!_houseInTheWay)
        {
            _house.transform.position = Vector3.Lerp(_house.transform.position, _startpoint.transform.position, _downSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _houseInTheWay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _houseInTheWay = false;
        }
    }
}
