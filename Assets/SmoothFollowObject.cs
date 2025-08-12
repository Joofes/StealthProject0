using UnityEngine;

public class SmoothFollowObject : MonoBehaviour
{
    public Transform follow;
public float speed = 5f; // movement speed

void Update()
{
    transform.position = Vector3.Lerp(
        transform.position,        // current position
        follow.position,           // target position
        speed * Time.deltaTime     // interpolation factor
    );
}
}
