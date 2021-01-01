using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class PlanetSubEditor : VisualElement
{
    Planet planet;
    OrbitLineSubEditor orbitLineSubEditor;

    public PlanetSubEditor(OrbitLineSubEditor orbitLineSubEditor, Planet planet)
    {
        this.orbitLineSubEditor = orbitLineSubEditor;
        this.planet = planet;

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/PlanetSubEditor.uxml");
        visualTree.CloneTree(this);

        StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/PlanetSubEditor.uss");
        this.styleSheets.Add(stylesheet);

        this.AddToClassList("planetSubeditor");

        #region Fields
        TextField nameField = this.Query<TextField>("planetName").First();
        nameField.value = planet.name;
        nameField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                planet.name = (string)e.newValue;
                EditorUtility.SetDirty(planet);
            }
        );

        FloatField scaleField = this.Query<FloatField>("planetScale").First();
        scaleField.value = planet.scale;
        scaleField.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                planet.scale = e.newValue;
                EditorUtility.SetDirty(planet);
            }
        );

        FloatField speedField = this.Query<FloatField>("planetSpeed").First();
        speedField.value = planet.speed;
        speedField.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                planet.speed = e.newValue;
                EditorUtility.SetDirty(planet);
            }
        );

        #endregion

        #region Buttons
        Button btnRemovePlanet = this.Query<Button>("btnRemove").First();
        btnRemovePlanet.clickable.clicked += RemovePlanet;
        #endregion
    }

    #region Button Functions
    private void RemovePlanet()
    {
        if (EditorUtility.DisplayDialog("Delete Planet", "Are you sure you want to delete this planet?", "Delete", "Cancel"))
            orbitLineSubEditor.RemovePlanet(planet);
    }
    #endregion
}
