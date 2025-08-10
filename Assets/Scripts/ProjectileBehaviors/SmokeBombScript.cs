using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBombScript : MonoBehaviour
{
    public GameObject smoke;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameObject newSmoke = Instantiate(smoke);
            newSmoke.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
