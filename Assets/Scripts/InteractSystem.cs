using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    private readonly Collider[] interactables = new Collider[3];
    int num;
    float radius = 2f;
    public LayerMask interactableLayer;
    void Update()
    {
        num = Physics.OverlapSphereNonAlloc(transform.position, radius, interactables, interactableLayer);

        if(num > 0 )
        {
            IInteractable interactable = interactables[0].GetComponent<IInteractable>();
            if(interactable != null && Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact(this);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
