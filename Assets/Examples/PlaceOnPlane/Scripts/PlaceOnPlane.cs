using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlaceOnPlane : MonoBehaviour, IDragHandler
{

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    public bool isOverGameScreen = true;

    public void OnDrag(PointerEventData pointerEventData)
    {

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(m_PlacedPrefab, pointerEventData.pointerCurrentRaycast.worldPosition, Quaternion.identity);
        }
        else if (isOverGameScreen)
        {
            spawnedObject.transform.position = pointerEventData.pointerCurrentRaycast.worldPosition;
        }

    }


}
