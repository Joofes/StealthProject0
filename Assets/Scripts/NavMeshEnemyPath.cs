using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemyPath : MonoBehaviour
{
    public GameObject path;
    Transform[] targetPoints;

    private NavMeshAgent agent;
    private int currentTargetIndex = 0;

    public bool idle = true;

    private void Start()
    {
        targetPoints = path.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        if (targetPoints.Length > 0)
            agent.SetDestination(targetPoints[currentTargetIndex].position);
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && idle)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Length;
            agent.SetDestination(targetPoints[currentTargetIndex].position);
        }
    }
}
