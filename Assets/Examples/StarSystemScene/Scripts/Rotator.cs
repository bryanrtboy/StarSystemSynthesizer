using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 m_rotation = Vector3.zero;
    public bool m_randomRotation = false;
    public Vector3 m_minRotation = -Vector3.one;
    public Vector3 m_maxRotation = Vector3.one;
    public float m_speed = 1.0f;
    public bool m_randomSpeed = false;
    public Vector2 m_minMaxSpeed = Vector2.up;
    public bool m_delayStart = false;
    public Vector2 m_minMaxDelay = Vector2.up;

    bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        if (m_delayStart)
        {
            isPlaying = false;
            Invoke("StartAfterDelay", Random.Range(m_minMaxDelay.x, m_minMaxDelay.y));

        }

        if (m_randomRotation)
            m_rotation = new Vector3(Random.Range(m_minRotation.x, m_maxRotation.x), Random.Range(m_minRotation.y, m_maxRotation.y), Random.Range(m_minRotation.z, m_maxRotation.z));


        if (m_randomSpeed)
            m_speed = Random.Range(m_minMaxSpeed.x, m_minMaxSpeed.y);

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
            this.transform.Rotate(m_rotation * Time.deltaTime * m_speed);
    }

    void StartAfterDelay()
    {
        isPlaying = true;
    }
}
