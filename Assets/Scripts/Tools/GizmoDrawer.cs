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

    public static void DrawBox(Vector3 center, Vector3 size)
    {
        Gizmos.color = Color.yellow;

        Vector2 halfSize = size / 2;

        // Define corners
        Vector2 topLeft = (Vector2) center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = (Vector2) center + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomRight = (Vector2) center + new Vector2(halfSize.x, -halfSize.y);
        Vector2 bottomLeft = (Vector2) center + new Vector2(-halfSize.x, -halfSize.y);

        // Draw box lines
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
