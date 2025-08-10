using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactLength;
    public GameObject cam;

    public LayerMask treasureLayer;

    public KeyCode interactKey;

    public PlayerStats stats;

    RaycastHit hit;
    void Update()
    {
        TreasureCheck();
    }

    void TreasureCheck()
    {
        Physics.Raycast(cam.transform.position, cam.transform.forward,out hit, interactLength,treasureLayer);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactLength, Color.green);
        if(hit.collider != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                stats.money = hit.collider.gameObject.GetComponent<TreasureScript>().value;
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
