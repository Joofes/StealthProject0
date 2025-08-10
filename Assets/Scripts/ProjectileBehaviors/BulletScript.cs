using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;
    public float speed;

    public GameObject kunaiParticle;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Enemy")
            collision.gameObject.GetComponent<Enemy>().TakeHit(damage); 
        if (collision.transform.tag == "EnemyHead")
        {
            Debug.Log("Headshot");
            collision.transform.parent.GetComponent<Enemy>().TakeHit(1000);   
        } 
        if(collision.transform.tag != "Player")
        {
            GameObject particle = Instantiate(kunaiParticle);
            particle.transform.position = transform.position;
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
        
            
    }
    void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
