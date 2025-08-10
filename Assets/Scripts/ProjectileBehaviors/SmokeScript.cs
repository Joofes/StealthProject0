using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{
    public float smokeTime;
    void Awake()
    {
        StartCoroutine(SmokeDestroy());
    }

    IEnumerator SmokeDestroy()
    {
        yield return new WaitForSeconds(smokeTime);
        Destroy(gameObject);
    }
}
