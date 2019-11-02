using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    [SerializeField] Camera weaponCamera;

    float rayDistance = 10f;
    Vector3 offset = new Vector3(0f, 1.5f, 0f);
    Vector3 rayDirection;
    CharacterController m_Controller;
    PlayerCharacterController m_PlayerController;

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<CharacterController, Tracer>(m_Controller, this, gameObject);

        m_PlayerController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Tracer>(m_Controller, this, gameObject);

    }

    private void Update()
    {
        Vector3 startPosition = transform.position + offset;
        rayDirection = transform.forward + offset;

        RaycastHit hit;
        if(Physics.Raycast(startPosition, rayDirection, out hit, rayDistance))
        {

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
        }
    }


}
