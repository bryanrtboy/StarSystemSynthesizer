using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class StarSystemController : MonoBehaviour
{
    public StarSystem starSystem;
    public GameObject notePrefab;
    public GameObject planetPrefab;
    public GameObject orbitPrefab;
    public UIDocument userControls;


    //[HideInInspector] [SerializeField] private GameObject starObject;
    [HideInInspector] [SerializeField] private List<GameObject> planetObjects = new List<GameObject>();
    [HideInInspector] [SerializeField] private List<GameObject> orbitObjects = new List<GameObject>();
    [HideInInspector] [SerializeField] private List<GameObject> noteObjects = new List<GameObject>();
    [HideInInspector] [SerializeField] private Dictionary<string, GameObject> synthObjects = new Dictionary<string, GameObject>();

    private VisualElement controls;

    void OnEnable()
    {
        if (userControls != null && userControls.rootVisualElement != null)
            UpdateSystem();

    }


    public void UpdateSystem()
    {
        ClearSystem();


        //Create Planet GameObjects
        foreach (OrbitLine orbit in starSystem.orbits)
        {
            GameObject newOrbitObject = Instantiate(orbitPrefab, transform.position, Quaternion.identity);
            newOrbitObject.transform.parent = gameObject.transform;

            newOrbitObject.name = orbit.name;

            OrbitController orbitControls = newOrbitObject.GetComponent<OrbitController>();
            orbitControls.useWaypoints = orbit.useWaypoints;


            Slider slider = new Slider();
            slider.name = orbit.name;
            slider.label = "";
            slider.value = orbit.orbitDistance;
            slider.highValue = 1.0f;
            slider.showInputField = true;

            slider.RegisterCallback<ChangeEvent<float>>(
                e =>
                {
                    if (orbitControls != null)
                        orbitControls.UpdateOrbit(e.newValue);

                }
            );

            controls.Add(slider);

            newOrbitObject.DrawOrbitLine(orbit.orbitDistance, orbit.segments, orbit.thickness);

            foreach (Note note in orbit.notes)
            {
                GameObject noteObject = Instantiate(notePrefab, transform.position, Quaternion.identity);
                noteObject.transform.parent = newOrbitObject.transform;
                noteObject.name = note.note.ToString();
                noteObject.transform.localPosition = new Vector3(0, 0, orbit.orbitDistance);

                //Create and extract a synth, checking so that only one synth exists when used by more than one note indicator
                if (note.synth != null && !synthObjects.ContainsKey(note.synth.name))
                {
                    GameObject g = Instantiate(note.synth, newOrbitObject.transform.position, Quaternion.identity, gameObject.transform);
                    synthObjects.Add(note.synth.name, g);
                }

                //Assign the synth instantiated to the note trigger
                if (note.synth != null && synthObjects.ContainsKey(note.synth.name))
                {
                    SendNoteOnTrigger trigger = noteObject.GetComponentInChildren<SendNoteOnTrigger>();
                    if (trigger != null)
                    {
                        //Set up all the note data
                        trigger.m_pentatonic = starSystem.pentatonic;

                        trigger.octave = note.octave;
                        trigger.velocity = note.velocity;
                        trigger.length = note.length;
                        trigger.pentatonicValue = note.pentatonicValue;

                        trigger.m_root = note.note;


                        //Find and attach the synth to the Note
                        GameObject helm;
                        synthObjects.TryGetValue(note.synth.name, out helm);

                        if (helm != null)
                        {
                            //AudioHelm.HelmController helmController = helm.GetComponent<AudioHelm.HelmController>();
                            //if (helmController != null)
                            //    trigger.m_sequencer = helmController;
                        }

                    }
                }

                //Scale the note indicator
                Transform[] children = noteObject.GetComponentsInChildren<Transform>();
                foreach (Transform g in children)
                    g.localScale = new Vector3(note.size, note.size, note.size);


                noteObjects.Add(noteObject);
                orbitControls.notes.Add(noteObject);

            }


            foreach (Planet planet in orbit.planets)
            {
                GameObject planetObject = Instantiate(planetPrefab, transform.position, Quaternion.identity);
                planetObject.transform.parent = newOrbitObject.transform;
                planetObject.transform.localEulerAngles = Vector3.zero;
                planetObject.name = planet.name;

                planetObject.transform.localPosition = new Vector3(0, 0, orbit.orbitDistance);

                Transform[] children = planetObject.GetComponentsInChildren<Transform>();

                foreach (Transform g in children)
                    g.localScale = new Vector3(planet.scale, planet.scale, planet.scale);

                if (!orbitControls.useWaypoints)
                {
                    RotateAroundOrigin rot = planetObject.AddComponent<RotateAroundOrigin>();
                    rot.target = newOrbitObject;
                    rot.radius = orbit.orbitDistance;
                    rot.speed = planet.speed;
                    orbitControls.planetRotators.Add(rot);
                }
                else
                {
                    MoveToWaypoints moveToWaypoints = planetObject.AddComponent<MoveToWaypoints>();
                    moveToWaypoints.wayPoints = orbitControls.notes.ToArray();
                    moveToWaypoints.speed = planet.speed;
                }

                planetObjects.Add(planetObject);
                orbitControls.planets.Add(planetObject);

            }

            //If you want to vary the starting angle, pass it in here.
            newOrbitObject.transform.localEulerAngles = Vector3.zero;

            orbitControls.UpdateWaypointPositions();
            orbitObjects.Add(newOrbitObject);
        }

    }

    private void ClearSystem()
    {
        //var root = userControls.gameObject.GetComponent<VisualElement>().rootVisualElement;

        var root = userControls.rootVisualElement;
        controls = root.Q("controls");
        controls.Clear();


        if (planetObjects.Count > 0)
            foreach (GameObject planet in planetObjects)
                DestroyImmediate(planet);

        planetObjects.Clear();

        if (noteObjects.Count > 0)
            foreach (GameObject note in noteObjects)
                DestroyImmediate(note);

        noteObjects.Clear();

        synthObjects = new Dictionary<string, GameObject>();

        //AudioHelm.HelmController[] synthInstances = this.transform.GetComponentsInChildren<AudioHelm.HelmController>();
        //foreach (AudioHelm.HelmController s in synthInstances)
        //    DestroyImmediate(s.gameObject);

        AudioSource[] synthInstances = this.transform.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource a in synthInstances)
            DestroyImmediate(a.gameObject);


        if (orbitObjects.Count > 0)
            foreach (GameObject orbit in orbitObjects)
                DestroyImmediate(orbit);


        orbitObjects.Clear();

    }
}
