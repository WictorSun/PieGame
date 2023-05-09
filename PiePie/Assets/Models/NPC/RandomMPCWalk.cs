using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMPCWalk : MonoBehaviour
{
    public Transform[] waypoints;
    private int _currentWaypointIndex;
    private Transform[] _originalWaypoints;
    [SerializeField] private int _amountWPLeft;
    public NavMeshAgent agent;

    public float _waitTime = 0f;
    private float _WaitCounter = 0f;
    private bool _waiting = false;

    

    private void Start()
    {
        _originalWaypoints = new Transform[waypoints.Length];
        waypoints.CopyTo(_originalWaypoints, 0);
        ShuffleWaypoints();
    }

    private void ShuffleWaypoints()
    {
        // Shuffle the waypoints using Fisher-Yates algorithm
        for (int i = waypoints.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform temp = waypoints[i];
            waypoints[i] = waypoints[j];
            waypoints[j] = temp;
        }
    }
    private void ResetWaypoints()
    {
        _currentWaypointIndex = 0;
        waypoints = _originalWaypoints;
        ShuffleWaypoints();
    }

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

            if (_currentWaypointIndex >= waypoints.Length)
            {
                ResetWaypoints(); // Shuffle the waypoints again
                //_currentWaypointIndex = 0;
            }

            transform.LookAt(wp.position);
        }
        else
        {
            agent.SetDestination(wp.transform.position);
            //transform.LookAt(wp.position);

        }
    }
}