using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public UIDocument userControls;

    void OnEnable()
    {
        var root = userControls.rootVisualElement;
        root?.Q("slider")?.RegisterCallback<ClickEvent>(ev => OpenScene(1));

        root?.Q("solar")?.RegisterCallback<ClickEvent>(ev => OpenScene(2));

        root?.Q("place")?.RegisterCallback<ClickEvent>(ev => OpenScene(3));
    }

    void OpenScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }

}
