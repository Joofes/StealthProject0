using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float closeRadius;
    [Range(0, 360)]
    public float radius;
    [Range(0,360)]
    public float angle;
    [Range(0, 360)]
    public float stealthAngle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask ropeMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool canSeeRope;
    public bool canStealthKill;
    public bool playerClose;

    public GameObject rope;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewPlayerCheck();
            FieldOfViewRopeCheck();
            StealthKillCheck();
            CloseRangePlayerCheck();
        }
    }

    private void FieldOfViewPlayerCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    void CloseRangePlayerCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, closeRadius, targetMask);
        if (rangeChecks.Length != 0)
        {
            playerClose = true;
        }
        else
        {
            playerClose = false;
        }
    }

    void FieldOfViewRopeCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, ropeMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget,  obstructionMask))
                {
                    canSeeRope = true;
                    rope = Physics.OverlapSphere(transform.position, radius, ropeMask)[0].gameObject;
                }  
                else
                    canSeeRope = false;
            }
            else
                canSeeRope = false;
        }
        else if (canSeeRope)
            canSeeRope = false;
    }

    void StealthKillCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, (radius/2), targetMask);
        if (rangeChecks.Length != 0)
        {
            GameObject player = rangeChecks[0].gameObject;
            Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
            if (Vector3.Angle(-transform.forward, directionToTarget) < stealthAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canStealthKill = true;
                else
                    canStealthKill = false;
            }
            else
                canStealthKill = false;
        }
    }
}
