using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class OrbitLineSubEditor : VisualElement
{

    OrbitLine orbitLine;
    StarSystemEditor starSystemEditor;
    private VisualElement planetList;
    private VisualElement noteList;


    public OrbitLineSubEditor(StarSystemEditor starSystemEditor, OrbitLine orbitLine)
    {
        this.starSystemEditor = starSystemEditor;
        this.orbitLine = orbitLine;

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/OrbitLineSubEditor.uxml");
        visualTree.CloneTree(this);

        StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/OrbitLineSubEditor.uss");
        this.styleSheets.Add(stylesheet);

        this.AddToClassList("orbitLineSubEditor");

        // Store visual element that will contain the planet sub-inspectors. 
        planetList = this.Query<VisualElement>("planetList").First();
        UpdatePlanets();

        noteList = this.Query<VisualElement>("noteList").First();
        UpdateNotes();


        #region Fields
        TextField stringField = this.Query<TextField>("orbitName").First();
        stringField.value = orbitLine.name;
        stringField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                orbitLine.name = (string)e.newValue;
                EditorUtility.SetDirty(orbitLine);
            }
        );

        Slider distanceField = this.Query<Slider>("orbitDistance").First();
        distanceField.value = orbitLine.orbitDistance;
        distanceField.label = "Orbit Distance " + distanceField.value.ToString("F2");
        distanceField.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                orbitLine.orbitDistance = e.newValue;
                distanceField.label = "Orbit Distance " + e.newValue.ToString("F2");
                EditorUtility.SetDirty(orbitLine);
            }
        );


        Slider thicknessField = this.Query<Slider>("thickness").First();
        thicknessField.value = orbitLine.thickness;
        thicknessField.label = "Thickness " + thicknessField.value.ToString("F3");
        thicknessField.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                orbitLine.thickness = e.newValue;
                thicknessField.label = "Thickness " + e.newValue.ToString("F3");
                EditorUtility.SetDirty(orbitLine);
            }
        );

        IntegerField segmentsField = this.Query<IntegerField>("segments").First();
        segmentsField.value = orbitLine.segments;
        segmentsField.RegisterCallback<ChangeEvent<int>>(
            e =>
            {
                orbitLine.segments = e.newValue;
                EditorUtility.SetDirty(orbitLine);
            }
        );

        ColorField colorField = this.Query<ColorField>("color").First();
        colorField.value = orbitLine.color;
        colorField.RegisterCallback<ChangeEvent<Color>>(
            e =>
            {
                orbitLine.color = e.newValue;
                EditorUtility.SetDirty(orbitLine);
            }
        );

        Toggle waypointField = this.Query<Toggle>("useWaypoints").First();
        waypointField.value = orbitLine.useWaypoints;
        waypointField.RegisterCallback<ChangeEvent<bool>>(
            e =>
            {
                orbitLine.useWaypoints = e.newValue;
                EditorUtility.SetDirty(orbitLine);
            }
        );

        #endregion

        #region Buttons
        // Assign methods to the click events of the two buttons.
        Button btnAddPlanet = this.Query<Button>("btnAddNewPlanet").First();
        btnAddPlanet.clickable.clicked += AddPlanet;

        Button btnRemoveAllPlanets = this.Query<Button>("btnRemoveAllPlanets").First();
        btnRemoveAllPlanets.clickable.clicked += RemoveAllPlanets;

        // Assign methods to the click events of the two buttons.
        Button btnAddNote = this.Query<Button>("btnAddNewNote").First();
        btnAddNote.clickable.clicked += AddNote;

        Button btnRemoveAllNotes = this.Query<Button>("btnRemoveAllNotes").First();
        btnRemoveAllNotes.clickable.clicked += RemoveAllNotes;



        Button btnRemoveOrbit = this.Query<Button>("btnRemoveOrbitLine").First();
        btnRemoveOrbit.clickable.clicked += RemoveOrbitLine;
        #endregion
    }



    #region Button Functions
    private void RemoveOrbitLine()
    {
        if (EditorUtility.DisplayDialog("Delete Planet", "Are you sure you want to delete this orbit?", "Delete", "Cancel"))
            starSystemEditor.RemoveOrbitLine(orbitLine);
    }

    private void AddPlanet()
    {
        Planet planet = ScriptableObject.CreateInstance<Planet>();
        planet.name = "New Planet";
        orbitLine.planets.Add(planet);
        AssetDatabase.AddObjectToAsset(planet, starSystemEditor.starSystem);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdatePlanets();
    }

    //Remove all the planets attached to this orbit
    private void RemoveAllPlanets()
    {
        if (EditorUtility.DisplayDialog("Delete All Planets", "Are you sure you want to delete all of the planets this orbit has?", "Delete All", "Cancel"))
        {
            for (int i = orbitLine.planets.Count - 1; i >= 0; i--)
            {
                AssetDatabase.RemoveObjectFromAsset(orbitLine.planets[i]);
                orbitLine.planets.RemoveAt(i);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdatePlanets();
        }
    }

    private void AddNote()
    {
        Note note = ScriptableObject.CreateInstance<Note>();
        note.name = "New Note";
        orbitLine.notes.Add(note);
        AssetDatabase.AddObjectToAsset(note, starSystemEditor.starSystem);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdateNotes();
    }

    //Remove all the planets attached to this orbit
    private void RemoveAllNotes()
    {
        if (EditorUtility.DisplayDialog("Delete All Notes", "Are you sure you want to delete all of the notes this orbit has?", "Delete All", "Cancel"))
        {
            for (int i = orbitLine.notes.Count - 1; i >= 0; i--)
            {
                AssetDatabase.RemoveObjectFromAsset(orbitLine.notes[i]);
                orbitLine.notes.RemoveAt(i);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateNotes();
        }
    }
    #endregion

    #region Display Planet Data Functions
    public void UpdatePlanets()
    {
        this.planetList.Clear();
        // Create and add a PlanetSubEditor to our planetList container for each planet.
        foreach (Planet planet in orbitLine.planets)
        {
            PlanetSubEditor planetSubEditor = new PlanetSubEditor(this, planet);
            this.planetList.Add(planetSubEditor);
        }
    }

    public void UpdateNotes()
    {
        this.noteList.Clear();
        // Create and add a PlanetSubEditor to our planetList container for each planet.
        foreach (Note note in orbitLine.notes)
        {
            NoteSubEditor noteSubEditor = new NoteSubEditor(this, note);
            this.noteList.Add(noteSubEditor);
        }
    }


    // Remove a specified Planet from the StarSystem asset, save the changes and update the Planet sub-inspectors.
    public void RemovePlanet(Planet planet)
    {
        orbitLine.planets.Remove(planet);
        AssetDatabase.RemoveObjectFromAsset(planet);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdatePlanets();
    }

    // Remove a specified Planet from the StarSystem asset, save the changes and update the Planet sub-inspectors.
    public void RemoveNote(Note note)
    {
        orbitLine.notes.Remove(note);
        AssetDatabase.RemoveObjectFromAsset(note);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdateNotes();
    }

    #endregion


}