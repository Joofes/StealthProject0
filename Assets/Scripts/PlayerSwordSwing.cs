using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSwing : MonoBehaviour
{
    public bool canSwing = true;
    public Transform hitBoxPos;

    public float hbSize;

    public int swordDamage;

    public Animator swordAnim;

    public LayerMask enemyLayer;
    public LayerMask glassLayer;

    public float stealthKillDistance;

    public float missParryCD;
    float parryCooldown;

    float comboTimerRefresh = 0.5f;
    float comboTimer;
    public string[] comboNames;
    int currentCombo;
    // Update is called once per frame
    void Update()
    {
        parryCooldown -= Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && canSwing)
        {
            StartCoroutine(Swing());
        }
        if(Input.GetMouseButtonDown(1) && parryCooldown <= 0)
        {
            TryParry();
        }
        comboTimer -= Time.deltaTime;
        if(comboTimer < 0)
        {
            currentCombo = 0;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitBoxPos.position, hbSize);
    }
    IEnumerator Swing()
    {
        canSwing = false;
        Collider[] enemiesHit = Physics.OverlapSphere(hitBoxPos.position, hbSize, enemyLayer);
        foreach (Collider hit in enemiesHit)
        {
            Debug.Log(hit);
            if (hit.GetComponent<FieldOfView>().canStealthKill)
            {  
                StartCoroutine(StealthKill(hit.gameObject));
                StopCoroutine(Swing());
                yield break;
            }
        }
        swordAnim.Play(comboNames[currentCombo]);
        Collider[] glass = Physics.OverlapSphere(hitBoxPos.position, hbSize, glassLayer);
        foreach (Collider hit in glass)
        {
            hit.GetComponent<GlassScript>().DestroyGlass();
        }
        yield return new WaitForSeconds(.33f);
        currentCombo = (currentCombo + 1) % comboNames.Length;
        comboTimer = comboTimerRefresh;
        if(enemiesHit.Length > 0)
        {
            foreach (Collider hit in enemiesHit)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeHit(swordDamage);
                }
                else
                {
                    Dummy dummy = hit.GetComponent<Dummy>();
                    dummy.TakeHit(swordDamage);
                }
            }
        }
        //yield return new WaitForSeconds(0.2f);
        //swordAnim.ResetTrigger("Swing");
        canSwing = true;
    }
    void TryParry()
    {
        swordAnim.Play("SwordParry");
        canSwing = false;
        parryCooldown = missParryCD;
        Collider[] enemiesHit = Physics.OverlapSphere(hitBoxPos.position, hbSize, enemyLayer);
        if (enemiesHit.Length > 0)
        {
            if(enemiesHit[0].GetComponent<Enemy>().canParry)
            {
                StartCoroutine(enemiesHit[0].GetComponent<Enemy>().Parry());
                
                parryCooldown = 0;
            }
        }
        canSwing = true;
    }
    public IEnumerator StealthKill(GameObject target)
    {
        
        StopCoroutine(Swing());
        swordAnim.ResetTrigger("Swing");

         canSwing = false;

        Vector3 targetPosition = target.transform.position;
        Quaternion targetRotation = target.transform.rotation;

        transform.position = targetPosition - stealthKillDistance * target.transform.forward;
        swordAnim.SetTrigger("Stealth");
        yield return new WaitForSeconds(0.275f);
        target.GetComponent<Enemy>().health = 0;
        swordAnim.ResetTrigger("Stealth");
        canSwing = true;
    }
}
