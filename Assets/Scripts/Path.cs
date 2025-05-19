using UnityEngine;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
    public List<Transform> controlPoints = new List<Transform>();

    /// <summary>
    /// Samples a point on the path based on t [0, 1]
    /// </summary>
    public Vector3 GetPoint(float t)
    {
        if (controlPoints.Count == 0)
            return transform.position;

        if (controlPoints.Count == 1)
            return controlPoints[0].position;

        // Clamp t
        t = Mathf.Clamp01(t);

        // Total segments
        int segments = controlPoints.Count - 1;
        float scaledT = t * segments;
        int i = Mathf.FloorToInt(scaledT);
        float segmentT = scaledT - i;

        if (i >= segments) i = segments - 1;

        Vector3 p0 = controlPoints[i].position;
        Vector3 p1 = controlPoints[i + 1].position;

        // Linear interpolation (can be upgraded to Catmull-Rom or Bézier)
        return Vector3.Lerp(p0, p1, segmentT);
    }

    /// <summary>
    /// Finds closest point on the path to a given world position
    /// </summary>
    public Vector3 GetClosestPoint(Vector3 worldPos, int resolution = 50)
    {
        float closestT = 0;
        float minDistance = float.MaxValue;
        Vector3 closestPoint = Vector3.zero;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 point = GetPoint(t);
            float dist = Vector3.Distance(worldPos, point);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestT = t;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            if (controlPoints[i] != null && controlPoints[i + 1] != null)
            {
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
            }
        }

        // Draw sampled curve
        Gizmos.color = Color.yellow;
        Vector3 prev = GetPoint(0);
        for (int i = 1; i <= 100; i++)
        {
            float t = i / 100f;
            Vector3 current = GetPoint(t);
            Gizmos.DrawLine(prev, current);
            prev = current;
        }
    }
}
