using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(StarSystem))]
public class StarSystemEditor : Editor
{
    private VisualElement rootElement;
    //private VisualElement planetList;
    private VisualElement orbitList;
    [HideInInspector] public StarSystem starSystem;

    public void OnEnable()
    {
        starSystem = (StarSystem)target;
        rootElement = new VisualElement();

        // Load in UXML template and USS styles, then apply them to the root element.
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/StarSystemEditor.uxml");
        visualTree.CloneTree(rootElement);

        StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/StarSystemEditor.uss");
        rootElement.styleSheets.Add(stylesheet);
    }

    public override VisualElement CreateInspectorGUI()
    {
        #region Fields

        EnumField pentatonic = rootElement.Query<EnumField>("pentatonic").First();
        pentatonic.value = starSystem.pentatonic;
        pentatonic.RegisterCallback<ChangeEvent<System.Enum>>(
             e =>
             {
                 starSystem.pentatonic = (PentatonicScale)e.newValue;
                 EditorUtility.SetDirty(starSystem);
             }
         );
        #endregion

        #region Display Orbit Data 
        // Store visual element that will contain the planet sub-inspectors. 
        orbitList = rootElement.Query<VisualElement>("orbitList").First();
        UpdateOrbitLines();
        #endregion

        #region Buttons
        // Assign methods to the click events of the two buttons.
        Button btnAddOrbit = rootElement.Query<Button>("btnAddNewOrbit").First();
        btnAddOrbit.clickable.clicked += AddOrbitLine;

        Button btnRemoveAllOrbits = rootElement.Query<Button>("btnRemoveAllOrbits").First();
        btnRemoveAllOrbits.clickable.clicked += RemoveAll;

        Button btnRefresh = rootElement.Query<Button>("refresh").First();
        btnRefresh.clickable.clicked += UpdateOrbitLines;
        #endregion


        return rootElement;
    }

    #region Display Orbit Data Functions
    public void UpdateOrbitLines()
    {
        orbitList.Clear();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // Create and add an OrbitLine Editor to our orbitlist container for each orbit.
        foreach (OrbitLine orbit in starSystem.orbits)
        {
            OrbitLineSubEditor orbitSubEditor = new OrbitLineSubEditor(this, orbit);
            orbitList.Add(orbitSubEditor);
        }
    }
    #endregion

    #region Button Functions

    // Create a new Orbit that is a child to the StarSystem asset. Save the assets to disk and update the Orbit sub-inspectors.
    private void AddOrbitLine()
    {
        OrbitLine orbitLine = ScriptableObject.CreateInstance<OrbitLine>();
        orbitLine.name = "New Orbit";
        starSystem.orbits.Add(orbitLine);
        AssetDatabase.AddObjectToAsset(orbitLine, starSystem);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdateOrbitLines();
    }

    private void RemoveAll()
    {
        if (EditorUtility.DisplayDialog("Delete All Orbits", "Are you sure you want to delete all of the orbits this star system has?", "Delete All", "Cancel"))
        {
            for (int i = starSystem.orbits.Count - 1; i >= 0; i--)
            {
                AssetDatabase.RemoveObjectFromAsset(starSystem.orbits[i]);
                starSystem.orbits.RemoveAt(i);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateOrbitLines();
        }
    }
    // Remove a specified Orbit from the StarSystem asset, save the changes and update the Orbit sub-inspectors.
    public void RemoveOrbitLine(OrbitLine orbit)
    {
        starSystem.orbits.Remove(orbit);
        AssetDatabase.RemoveObjectFromAsset(orbit);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdateOrbitLines();
    }
    #endregion
}