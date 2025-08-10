using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassScript : MonoBehaviour
{
    public float soundRadius;
    public LayerMask enemyLayer;
    public void DestroyGlass()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, soundRadius, enemyLayer);
        if (enemies.Length > 0)
        {
            foreach (Collider enemy in enemies)
            {
                enemy.GetComponent<Enemy>().soundDetection(transform.position);
            }
        }
        Destroy(gameObject);
    }
}
