using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{

    public LayerMask enemyLayer;
    public float soundRadius;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, soundRadius, enemyLayer);
            if(enemies.Length > 0)
            {
                foreach(Collider enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().soundDetection(transform.position);
                }
            }
            if (collision.transform.tag == "Ground")
            {
                Destroy(gameObject);
            }
           
        }
    }
}
