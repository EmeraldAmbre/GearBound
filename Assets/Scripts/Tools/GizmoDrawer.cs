using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GizmoDrawer
{
    public static void DrawCircle(Vector3 center, float radius)
    {
        Color gizmoColor = Color.yellow;
        int segments = 100;  // Number of segments to smooth the circle

        float angleStep = 360f / segments;
        Vector3 lastPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            float radian = angle * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0);
            Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }
}
