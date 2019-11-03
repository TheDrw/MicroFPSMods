using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// --- blink notes ---
/// no invul during blink
/// blinks through enemies and friends
/// can blink onto things at eye level
/// when on jump pads, blink cancels physics applied
/// blink keeps momentum of direction blink when in air
/// able to blink onto stairs/inclines and keeps same traveled distance as when on flat surface. ~45 degree surfaces
/// can blink over small obstructions that are below eye level
/// at a certain height, blink doesn't "teleport" onto surface but hang in the air then land. maybe about +3m
/// can blink onto platforms/spots that aren't connected
/// can not blink out of traps ( ex: junkrat ) not sure if will implement that
/// can blink as fast as pressing down key. no noticeable recovery frames
/// </summary>
public class Tracer : MonoBehaviour
{
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject blinkDistanceVisual;

    RaycastHit hit;
    CharacterController m_Controller;
    PlayerCharacterController m_PlayerController;

    float blinkDistance = 7.5f;
    Vector3 blinkDirection; // direction changes on the 
    Vector3 blinkStartingPosition;
    Vector3 blinkDirectionOffsetFromCharacter; // so the raycast doesn't collide with ourself
    Vector3 blinkHeightRestrictionOffset = new Vector3(0f, 1.5f, 0f);

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<CharacterController, Tracer>(m_Controller, this, gameObject);

        m_PlayerController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Tracer>(m_Controller, this, gameObject);

        blinkStartingPosition = blinkDistanceVisual.transform.position;
    }

    private void Update()
    {
        blinkDirection = transform.forward; // temp fwd for now
        blinkDirectionOffsetFromCharacter = blinkDirection * m_Controller.radius;
        Vector3 startPosition = transform.position + blinkHeightRestrictionOffset + blinkDirectionOffsetFromCharacter;

        Debug.DrawRay(startPosition, blinkDirection * blinkDistance, Color.red);
        if(Physics.Raycast(startPosition, blinkDirection, out hit, blinkDistance))
        {
            //print(hit.point);
            blinkDistanceVisual.transform.position = hit.point;
        }
        else
        {
            blinkDistanceVisual.transform.position = transform.position + blinkDirection * blinkDistance + blinkHeightRestrictionOffset;
        }
    }

    void Blink(Vector3 direction)
    {

    }
}
