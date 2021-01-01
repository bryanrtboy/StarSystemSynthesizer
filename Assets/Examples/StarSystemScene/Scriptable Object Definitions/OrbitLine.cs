using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLine : ScriptableObject
{
    public float orbitDistance = .1f;
    public float thickness = .001f;
    public int segments = 36;
    public bool useWaypoints = false;
    public Color color = Color.white;
    public List<Planet> planets;
    public List<Note> notes;


    private void OnEnable()
    {
        if (planets == null)
            planets = new List<Planet>();

        if (notes == null)
            notes = new List<Note>();
    }
}
