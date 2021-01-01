using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class NoteSubEditor : VisualElement
{
    Note note;

    OrbitLineSubEditor orbitLineSubEditor;

    public NoteSubEditor(OrbitLineSubEditor orbitLineSubEditor, Note note)
    {
        this.orbitLineSubEditor = orbitLineSubEditor;
        this.note = note;

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/NoteSubEditor.uxml");
        visualTree.CloneTree(this);

        StyleSheet stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Examples/StarSystemScene/Scripts/Editor/StarSystemEditor/NoteSubEditor.uss");
        this.styleSheets.Add(stylesheet);

        this.AddToClassList("noteSubeditor");


        #region Fields
        SliderInt octave = this.Query<SliderInt>("octave").First();
        octave.value = note.octave;
        octave.RegisterCallback<ChangeEvent<int>>(
            e =>
            {
                note.octave = e.newValue;
                EditorUtility.SetDirty(note);
            }
        );

        // Find an object field with the name and set the type
        ObjectField synthPrefab = this.Query<ObjectField>("synthPrefab").First();
        synthPrefab.objectType = typeof(GameObject);
        synthPrefab.value = note.synth;

        synthPrefab.RegisterCallback<ChangeEvent<Object>>(
            e =>
            {
                note.synth = (GameObject)e.newValue;
                EditorUtility.SetDirty(note);
            }
            );

        Slider velocity = this.Query<Slider>("velocity").First();
        velocity.value = note.velocity;
        velocity.label = "Velocity " + velocity.value.ToString("F3");
        velocity.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                note.velocity = e.newValue;
                velocity.label = "Velocity " + e.newValue.ToString("F3");
                EditorUtility.SetDirty(note);
            }
        );

        Slider length = this.Query<Slider>("length").First();
        length.value = note.length;
        length.label = "Length " + length.value.ToString("F3");
        length.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                note.length = e.newValue;
                length.label = "Length " + e.newValue.ToString("F3");
                EditorUtility.SetDirty(note);
            }
        );

        Slider size = this.Query<Slider>("size").First();
        size.value = note.size;
        size.label = "Size " + size.value.ToString("F3");
        size.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                note.size = e.newValue;
                size.label = "Size " + e.newValue.ToString("F3");
                EditorUtility.SetDirty(note);
            }
        );

        EnumField noteValue = this.Query<EnumField>("noteValue").First();
        noteValue.value = note.note;
        noteValue.RegisterCallback<ChangeEvent<System.Enum>>(
             e =>
             {
                 note.note = (NoteValue)e.newValue;
                 EditorUtility.SetDirty(note);
             }
         );

        #endregion


        #region Buttons
        Button btnRemoveNote = this.Query<Button>("btnRemove").First();
        btnRemoveNote.clickable.clicked += RemoveNote;
        #endregion
    }

    #region Button Functions
    private void RemoveNote()
    {
        if (EditorUtility.DisplayDialog("Delete Note", "Are you sure you want to delete this note?", "Delete", "Cancel"))
            orbitLineSubEditor.RemoveNote(note);
    }
    #endregion
}