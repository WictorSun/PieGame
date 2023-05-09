using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int _currentWaypointIndex;

    public NavMeshAgent agent;
    
    public float totalWaypoints = 2f;

    public float _waitTime = 0f;
    private float _WaitCounter = 0f;
    private bool _waiting = false;



    private void Update()
    {
        if (_waiting)
        {
            _WaitCounter += Time.deltaTime;
            if (_WaitCounter < _waitTime)
                return;
            _waiting = false;
        }

        Transform wp = waypoints[_currentWaypointIndex];
        if (Vector3.Distance(agent.transform.position, wp.position) < 10f)
        {
            //agent.transform.position = wp.position;
            _WaitCounter = 0f;
            _waiting = true;

            _currentWaypointIndex = (_currentWaypointIndex + 1);
            Debug.Log(_currentWaypointIndex);

            if (_currentWaypointIndex > totalWaypoints)
            {
                _currentWaypointIndex = 0;
            }

            transform.LookAt(wp.position);
        }
        else
        {
            agent.SetDestination(wp.transform.position); //Vector3.MoveTowards(transform.position, wp.position, _speed * Time.deltaTime);
            //transform.LookAt(wp.position);
            
        }
    }
}
