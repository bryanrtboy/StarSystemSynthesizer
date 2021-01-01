using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class CommonControls : MonoBehaviour
{
    public UIDocument userControls;

    //private VisualElement controls;

    public int sceneID = 0;

    void OnEnable()
    {
        var root = userControls.rootVisualElement;
        root?.Q("close")?.RegisterCallback<ClickEvent>(ev => OpenScene());
    }

    void OpenScene()
    {
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
    }


}
