using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundOrigin : MonoBehaviour
{
    public GameObject target;
    public float radius = .5f;
    public float speed = 1f;


    bool clockwiseRotation = true;

    private void Start()
    {
        if (speed < 0)
            clockwiseRotation = false;
        transform.localPosition = new Vector3(0, 0, radius);
    }

    void Update()
    {
        float angle = Mathf.Abs(speed * Mathf.Lerp(10f, .1f, radius)) * Time.deltaTime;
        Vector3 verticalaxis = transform.TransformDirection(Vector3.up);
        if (!clockwiseRotation)
            verticalaxis = transform.TransformDirection(Vector3.down);


        transform.RotateAround(target.transform.position, verticalaxis, angle);

    }

    public void UpdateOrbitDistance(float r)
    {
        transform.Translate(new Vector3(0, 0, r - radius), Space.Self);
        radius = r;
    }

}
