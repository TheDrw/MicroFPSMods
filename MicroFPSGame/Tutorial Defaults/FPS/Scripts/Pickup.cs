using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Pickup : MonoBehaviour
{
    [Tooltip("Sound played on pickup")]
    public AudioClip pickupSFX;
    [Tooltip("VFX spawned on pickup")]
    public GameObject pickupVFXPrefab;

    public UnityAction<PlayerCharacterController> onPick;
    public Rigidbody pickupRigidbody { get; private set; }

    SphereCollider m_SphereCollider;
    Vector3 m_StartPosition;
    bool m_HasPlayedFeedback;

    private void Start()
    {
        pickupRigidbody = GetComponent<Rigidbody>();
        DebugUtility.HandleErrorIfNullGetComponent<Rigidbody, Pickup>(pickupRigidbody, this, gameObject);
        m_SphereCollider = GetComponent<SphereCollider>();
        DebugUtility.HandleErrorIfNullGetComponent<SphereCollider, Pickup>(m_SphereCollider, this, gameObject);

        // ensure the physics setup is a kinematic rigidbody trigger
        pickupRigidbody.isKinematic = true;
        m_SphereCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Finally not comparing by tag. <3
        PlayerCharacterController pickingPlayer = other.GetComponent<PlayerCharacterController>();
        if (pickingPlayer != null)
        {
            if (onPick != null)
            {
                onPick.Invoke(pickingPlayer);
            }
        }
    }

    public void PlayPickupFeedback()
    {
        if (m_HasPlayedFeedback)
            return;

        if (pickupSFX)
        {
            AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
        }

        if (pickupVFXPrefab)
        {
            var pickupVFXInstance = Instantiate(pickupVFXPrefab, transform.position, Quaternion.identity);
        }

        m_HasPlayedFeedback = true;
    }
}
