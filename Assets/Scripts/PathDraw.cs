using UnityEngine;

public class PathDraw : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        DrawLines();
    }

    private void OnDrawGizmosSelected()
    {
        DrawLines();
    }

    private void DrawLines()
    {
        Transform[] childObjects = GetComponentsInChildren<Transform>();

        if (childObjects.Length < 3) // We need at least 2 child objects to draw a line
            return;

        Gizmos.color = Color.red;

        for (int i = 1; i < childObjects.Length - 1; i++)
        {
            Vector3 startPos = childObjects[i].position;
            Vector3 endPos = childObjects[i + 1].position;
            Gizmos.DrawLine(startPos, endPos);
        }
    }
}