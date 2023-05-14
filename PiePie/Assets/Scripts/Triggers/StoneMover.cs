using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class StoneMover : MonoBehaviour
{
    [SerializeField] private GameObject _stoneDoor;

    [SerializeField] private Transform _endPos;
    [SerializeField] private float _speed;

    public static bool _moveOnInGameLevel1;

    private void Update()
    {
        if (_moveOnInGameLevel1)
        {
            _stoneDoor.transform.position = Vector3.Lerp(_stoneDoor.transform.position, _endPos.transform.position, _speed * Time.deltaTime);
        }
    }
    [YarnCommand("openTheStoneDoor")]
    public static void OpenStoneDoor()
    {
        _moveOnInGameLevel1 = true;
    }
}
