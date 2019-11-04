using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// --- rewind notes ---
/// 
/// 
/// --- blink notes ---
/// no invul during blink
/// 
/// blinks through enemies and friends
/// 
/// can blink onto things at eye level
/// 
/// when on jump pads, blink cancels physics applied
/// 
/// able to blink onto stairs/inclines and keeps same traveled distance as when on flat surface. ~45 degree surfaces
/// 
/// can blink over small obstructions that are below eye level
/// 
/// at a certain height, blink doesn't "teleport" onto surface but hang in the air then land. maybe about +3m
/// 
/// can blink onto platforms/spots that aren't connected
/// 
/// can not blink out of traps ( ex: junkrat ) not sure if will implement that
/// 
/// can blink after only when current blink is done. can spam it, but they don't interrupt one another and can only be done in series.
/// 
/// To have an airborne blink, you have to be about ~1m higher than your landing height. 
/// When standing on one stair then jump in the training room, it lets me be airborne.
///     
/// keeps momemtum of direction before blinking. so if you are moving backward and you blink fwd, 
/// you will continue the backward momentum.
///     
/// When blinking through corners, it actually collides wiht it and will change the end posiiton. 
/// so if you are peaking a corner and half of your view is obstructed by a wall, 
/// your hitbox will displace the end position by the obstruction. 
/// this means blinking by using Vector3.MoveTowards isn't good enough.
/// 
/// </summary>
public class Tracer : MonoBehaviour
{
    [SerializeField] Camera weaponCamera;
    [SerializeField] GameObject blinkDistanceVisual;
    [SerializeField] float blinkSpeed = 500f;

    RaycastHit hit;
    CharacterController m_Controller;
    PlayerCharacterController m_PlayerController;
    PlayerInputHandler m_PlayerInputHandler;

    float blinkDistance = 7.5f;
    Vector3 blinkDirection; // direction changes on the 
    Vector3 blinkStartingPosition;
    Vector3 blinkDirectionOffsetFromCharacter; // so the raycast doesn't collide with ourself
    Vector3 blinkHeightRestrictionOffset = new Vector3(0f, 1.5f, 0f);
    bool isBlinking;

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<CharacterController, Tracer>(m_Controller, this, gameObject);

        m_PlayerController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Tracer>(m_Controller, this, gameObject);

        m_PlayerInputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Tracer>(m_Controller, this, gameObject);

        blinkStartingPosition = blinkDistanceVisual.transform.position;
    }

    private void Update()
    {
        blinkDirection = HandleBlinkDirection();
        blinkDirectionOffsetFromCharacter = blinkDirection * m_Controller.radius;
        Vector3 startPosition = transform.position + blinkHeightRestrictionOffset + blinkDirectionOffsetFromCharacter;

        Debug.DrawRay(startPosition, blinkDirection * blinkDistance, Color.red);
        if(Physics.Raycast(startPosition, blinkDirection, out hit, blinkDistance))
        {
            blinkDistanceVisual.transform.position = hit.point;
        }
        else
        {
            blinkDistanceVisual.transform.position = transform.position + blinkDirection * blinkDistance + blinkHeightRestrictionOffset;
        }

        // temp key input for skill
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Blink(Vector3.forward);
        }
    }

    // blink direction depeds on the current input's direction. 
    // if none is present, just blink fwd.
    Vector3 HandleBlinkDirection()
    {
        Vector3 playerCurrentDireciton = m_PlayerInputHandler.GetMoveInput() == Vector3.zero ?
            transform.forward :
            transform.TransformDirection(m_PlayerInputHandler.GetMoveInput());

        return playerCurrentDireciton;
    }

    void Blink(Vector3 direction)
    {
        if (isBlinking) return;

        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        m_PlayerController.ResetCharacterVelocity();
        m_PlayerController.enabled = false;
        isBlinking = true;
        Vector3 endPosition = new Vector3(
            blinkDistanceVisual.transform.position.x, 
            transform.position.y, 
            blinkDistanceVisual.transform.position.z);

        while(Vector3.Distance(transform.position, endPosition) > 0.1f)
        {
            print(Vector3.Distance(transform.position, endPosition));
            transform.position = Vector3.MoveTowards(transform.position, endPosition, blinkSpeed * Time.deltaTime);
            yield return null;
        }

        isBlinking = false;
        m_PlayerController.enabled = true;
    }
}
