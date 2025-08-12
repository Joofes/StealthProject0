using UnityEngine;
using UnityEngine.UIElements;

public class PointTowards : MonoBehaviour
{
    public GameObject target;
    void Update()
    {
        transform.LookAt(target.transform); 
    }
}
