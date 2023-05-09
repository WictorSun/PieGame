using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCStopTrigger : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject _player;
    [SerializeField] private Collider _detectPlayerTrigger;
    [SerializeField] private GameManager _GM;
    public bool _isInTrigger;
    private void Update()
    {
        if (_isInTrigger)
        {
            StartCoroutine(GoToPlayer(1f));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isInTrigger = true;
        }
    }
    IEnumerator GoToPlayer(float sec)
    {
        _GM._isInDia = true;
        yield return new WaitForSeconds(sec);
        agent.SetDestination(_player.transform.position);
        _detectPlayerTrigger.enabled = false;
    }
}
