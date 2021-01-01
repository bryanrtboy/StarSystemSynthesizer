using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PlacementInitializer : MonoBehaviour
{
    public GameObject thingToPlace;
    public UIDocument placementControls;

    PlaceOnPlane placement;

    void Start()
    {

        placement = FindObjectOfType<PlaceOnPlane>();

        var root = placementControls.rootVisualElement;
        root?.Q("done")?.RegisterCallback<ClickEvent>(ev => MakeTheThing());

        root?.Q("screen")?.RegisterCallback<PointerDownEvent>(ev => DownOverScreen());

        root?.Q("screen")?.RegisterCallback<PointerUpEvent>(ev => UpOverScreen());
    }

    void DownOverScreen()
    {
        if (placement)
            placement.isOverGameScreen = true;

    }

    void UpOverScreen()
    {
        if (placement)
            placement.isOverGameScreen = false;

    }

    void MakeTheThing()
    {
        if (placement)
            placement.isOverGameScreen = false;

        Instantiate(thingToPlace, transform.position, transform.rotation);

        DisablePlanesAndManager();
        Destroy(this.gameObject);
    }

    void DisablePlanesAndManager()
    {

        if (placement)
            Destroy(placement);
    }

}
