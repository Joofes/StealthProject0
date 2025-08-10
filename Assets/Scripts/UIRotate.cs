using UnityEngine;

public class UIRotate : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;

    private void Update()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y; // Locking the target's y-coordinate to the object's y-coordinate

        Vector3 directionToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}