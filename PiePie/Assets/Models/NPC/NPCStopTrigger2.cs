using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPCStopTrigger2 : MonoBehaviour
{
    [SerializeField] private YarnInteractable _YI;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] private Collider _startConvoTrigger;
    [SerializeField] private NPCStopTrigger _NT;
    public static bool _startTalking;
    public bool _canWalk;
    public bool _startWalking;

    private void Update()
    {
        if (_startTalking)
        {
            _YI.OnMouseDown();
            _canWalk = true;
        }
        if(!_startTalking && _canWalk)
        {
            _startWalking = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _NT._isInTrigger = false;
            _startTalking = true;
            _startConvoTrigger.enabled = false;
            
        }
    }
    [YarnCommand("StartWalking")]
    public static void StartWalkingNpc()
    {
        _startTalking = false;
    }
}
