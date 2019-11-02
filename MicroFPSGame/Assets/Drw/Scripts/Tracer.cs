using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject blinkDistanceVisual;

    float blinkDistance = 7.5f;
    Vector3 heightOffset = new Vector3(0f, 1.5f, 0f);
    Vector3 blinkDirection;
    CharacterController m_Controller;
    PlayerCharacterController m_PlayerController;
    Vector3 blinkStartingPos;
    RaycastHit hit;

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<CharacterController, Tracer>(m_Controller, this, gameObject);

        m_PlayerController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Tracer>(m_Controller, this, gameObject);

        blinkStartingPos = blinkDistanceVisual.transform.position;
    }

    private void Update()
    {
        Vector3 startPosition = transform.position + heightOffset + transform.forward * m_Controller.radius;
        blinkDirection = transform.forward;

        Debug.DrawRay(startPosition, blinkDirection * blinkDistance, Color.red);
        if(Physics.Raycast(startPosition, blinkDirection, out hit, blinkDistance))
        {
            print(hit.point);
            blinkDistanceVisual.transform.position = hit.point;
        }
        else
        {
            blinkDistanceVisual.transform.position = transform.position + blinkDirection * blinkDistance + heightOffset;
        }
    }
}
