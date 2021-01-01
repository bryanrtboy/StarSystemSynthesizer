using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Circle
{
    public static void DrawCircle(this GameObject container, float radius, float lineWidth, int segments, Material lineMaterial)
    {
        //var segments = 360;
        var line = container.AddComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments;
        line.material = lineMaterial;
        line.loop = true;

        var pointCount = segments; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    public static void DrawSimpleCircle(this GameObject container, float radius, int segments)
    {
        //var segments = 360;
        var line = container.GetComponent<LineRenderer>();
        line.positionCount = segments;

        var pointCount = segments;
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    public static void DrawOrbitLine(this GameObject container, float radius, int segments, float lineWidth)
    {
        //var segments = 360;
        var line = container.GetComponent<LineRenderer>();
        line.positionCount = segments;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        var pointCount = segments;
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

    public static void UpdateLinePointsToCircle(this GameObject container, float radius, LineRenderer line)
    {
        int segments = line.positionCount;

        var pointCount = segments; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }

}

