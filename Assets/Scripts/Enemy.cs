using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, EnemyClass
{
    [Header("Detection")]
    public float detectionLimit = 100f;
    public float detectionRate = 0.5f;
    public float chaseRadius;
    [Range(0, 100)]
    public float currentDetection;
    bool chasing;
    public Slider detectionSlider;
    [Header("Attacking")]
    public bool canAtk = true;
    public Animator anim;
    public Transform hbPoint;
    public float hbRadius, alertRadius;
    public LayerMask playerLayer, enemyLayer;

    public bool ropeChasing;

    public bool canParry;
    public bool stun;
    public GameObject parryIndicator;

    public int health;

    public GameObject player;

     FieldOfView fov;

     NavMeshEnemyPath path;

     NavMeshAgent agent;

    private void Start()
    {
        path = GetComponent<NavMeshEnemyPath>();
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        path.idle = true;
        detectionSlider = GetComponentInChildren<Slider>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        detectionSlider = GetComponentInChildren<Slider>();
        detectionSlider.maxValue = detectionLimit;
        parryIndicator.SetActive(false);
    }
    private void Update()
    {
        DetectionHandler();

        if (canAtk && agent.remainingDistance <= agent.stoppingDistance && chasing && !stun)
        {

            StartCoroutine(AttackPlayer());
        }
        if(chasing)
        {
            detectionSlider.value = detectionSlider.maxValue;
            agent.stoppingDistance = 2f;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.stoppingDistance = 0.5f;
        }
        if(health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if (fov.canSeeRope && !stun && fov.rope != null)
        {
            ropeChasing = true;
            path.idle = false;
            agent.SetDestination(fov.rope.transform.position);
        }
        if (agent.remainingDistance <= agent.stoppingDistance && ropeChasing)
        {
            ropeChasing = false;

            fov.canSeeRope = false;
            path.idle = true;
            Destroy(fov.rope);
        }
        if(chasing && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y; // Locking the target's y-coordinate to the object's y-coordinate

            Vector3 directionToTarget = targetPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 500);
        }
    }
    public void soundDetection(Vector3 position)
    {
        if(!chasing && !stun)
        {
            path.idle = false;
            agent.SetDestination(position);
            agent.stoppingDistance = 1.5f;
            StartCoroutine(investigateNoise());
        }
        
    }
    IEnumerator investigateNoise()
    {
        while(agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            yield return new WaitForSeconds(3f);
            path.idle = true;
        }
    }
    void DetectionHandler()
    {
        detectionSlider.value = currentDetection;
        if (fov.playerClose && (currentDetection < detectionLimit))
        {
            currentDetection += detectionRate * Time.deltaTime;
            Mathf.Clamp(currentDetection, 0, detectionLimit);
        }
        if (fov.canSeePlayer && currentDetection < detectionLimit)
        {
            currentDetection += detectionRate * Time.deltaTime;
            Mathf.Clamp(currentDetection, 0, detectionLimit);
        }
        if (currentDetection >= detectionLimit)
        {  
            agent.SetDestination(player.transform.position);
            AlertNearby();
            chasing = true;
            path.idle = false;
        }
        if(!fov.canSeePlayer && !fov.playerClose && currentDetection > 0)
        {
            currentDetection -= detectionRate * Time.deltaTime;
            Mathf.Clamp(currentDetection, 0, detectionLimit);
        }
    }
    public void TakeHit(int damage)
    {
        health -= damage;
        if(health <= 0)
            Destroy(gameObject);
        else
            currentDetection = detectionLimit;
        
    }
    void AlertNearby()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, alertRadius, enemyLayer);
        foreach (Collider hit in enemies)
        {
            EnemyClass enemy = GetComponent<EnemyClass>();
            if (enemy != null)
            {
                enemy.Alert();
            }
        }
    }

    public void Alert()
    {
        agent.SetDestination(player.transform.position);
        chasing = true;
        path.idle = false;
    }
    IEnumerator AttackPlayer()
    {
        canAtk = false;
        anim.SetTrigger("EnemySwing");
        StartCoroutine(ParryWindow());
        yield return new WaitForSeconds(1.2f);
        Collider[] playerHit = Physics.OverlapSphere(hbPoint.position, hbRadius, playerLayer);
        Debug.Log(playerHit.Length);
        if (playerHit.Length > 0) 
        {
            player.GetComponent<PlayerStats>().health -= 5;
        }
        yield return new WaitForSeconds(0.3f);
        anim.ResetTrigger("EnemySwing");
        canAtk = true;
    }
    IEnumerator ParryWindow()
    {
        yield return new WaitForSeconds(1.05f);
        parryIndicator.SetActive(true);
        canParry = true;
        yield return new WaitForSeconds(.4f);
        canParry = false;
        parryIndicator.SetActive(false);
    }
    public IEnumerator Parry()
    {
        stun = true;
        canParry = false;
        anim.SetTrigger("Stun");
        StopAllCoroutines();
        parryIndicator.SetActive(false);
        yield return new WaitForSeconds(3f);
        stun = false;
        if(anim != null)
            anim.ResetTrigger("Stun");
        canAtk = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hbPoint.position, hbRadius);
    }
}
