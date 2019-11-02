using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Perform simple animations on a game object such as spinning and bobbing.
public class GameObjectSimpleAnimation : MonoBehaviour
{
    [Tooltip("Frequency at which the item will move up and down")]
    public float verticalBobFrequency = 1f;
    [Tooltip("Distance the item will move up and down")]
    public float bobbingAmplitude = 1f;
    [Tooltip("Rotation angle per second")]
    public float rotatingSpeed = 360f;

    Vector3 m_StartPosition;

    void Start()
    {
        // Remember start position for animation
        m_StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float bobbingAnimationPhase = ((Mathf.Sin(Time.time * verticalBobFrequency) * 0.5f) + 0.5f) * bobbingAmplitude;
        transform.position = m_StartPosition + Vector3.up * bobbingAnimationPhase;

        transform.Rotate(Vector3.up, rotatingSpeed * Time.deltaTime, Space.Self);
    }
}
