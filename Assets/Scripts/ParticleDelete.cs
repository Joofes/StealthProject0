using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDelete : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Check if the particle system has finished emitting
        if (!particleSystem.IsAlive(true))
        {
            // Delete the GameObject after the particles are done
            Destroy(gameObject);
        }
    }
}
