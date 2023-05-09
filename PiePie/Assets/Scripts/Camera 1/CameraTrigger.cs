using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _cam1;
    [SerializeField] private GameObject _cam2;
    [SerializeField] private GameObject _switchAnim;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _newPLayerPosition;

    public static bool _turnOn;
    private bool _go;

    private void Update()
    {
        if (_turnOn)
        {
            _cam1.SetActive(false);
            _cam2.SetActive(true);
            StartCoroutine(TurnOnAnimation(2.5f));
            StartCoroutine(MovePlayer(1f, .6f));
        }
        else if (_go )
        {
            _cam1.SetActive(false);
            _cam2.SetActive(true);
        }
        //else ()
        //{
        //    _cam1.SetActive(true);
        //    _cam2.SetActive(false);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //_turnOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           // _turnOn = false;
           // _go = false;
        }
    }
    [YarnCommand("StartTutorial")]
    public static bool StartTutorial()
    {
        return _turnOn = true;
    }
    IEnumerator TurnOnAnimation(float sec)
    {
        _switchAnim.SetActive(true);
        yield return new WaitForSeconds(sec);
        _switchAnim.SetActive(false);
    }
    IEnumerator MovePlayer(float sec, float sec2)
    {
        yield return new WaitForSeconds(sec);
        _player.transform.position = _newPLayerPosition.transform.position;
        yield return new WaitForSeconds(sec2);
        _turnOn = false;
        _go = true;

    }
}
