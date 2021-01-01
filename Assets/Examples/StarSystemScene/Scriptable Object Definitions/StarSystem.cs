using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Star System", menuName = "Star System")]
public class StarSystem : ScriptableObject
{
    //public Sprite sprite;
    //public List<Planet> planets;
    public List<OrbitLine> orbits;
    //public GameObject planetPrefab;
    //public GameObject notePrefab;
    //public GameObject orbitPrefab;
    public PentatonicScale pentatonic;

    private void OnEnable()
    {
        //if (planets == null)
        //    planets = new List<Planet>();

        if (orbits == null)
            orbits = new List<OrbitLine>();
    }
}
