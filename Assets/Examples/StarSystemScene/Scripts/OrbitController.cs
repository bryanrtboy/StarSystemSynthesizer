using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    public List<GameObject> notes = new List<GameObject>();
    public List<GameObject> planets = new List<GameObject>();
    public List<RotateAroundOrigin> planetRotators = new List<RotateAroundOrigin>();
    public bool useWaypoints = false;

    LineRenderer lineRenderer;

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateWaypointPositions();
    }

    public void UpdateOrbit(float radius)
    {
        foreach (RotateAroundOrigin p in planetRotators)
            p.UpdateOrbitDistance(radius);

        if (lineRenderer != null)
            this.gameObject.UpdateLinePointsToCircle(radius, lineRenderer);

        UpdateWaypointPositions();

    }

    public void UpdateWaypointPositions()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null || notes.Count < 1)
            return;

        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);

        int increment = positions.Length / notes.Count;

        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].transform.localPosition = positions[i * increment] + transform.localPosition;
        }

    }

}
