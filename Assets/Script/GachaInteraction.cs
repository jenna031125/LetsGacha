using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class GachaInteraction : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool hasBeenPickedUp = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Ensure it starts solid and frozen in the slot
        rb.isKinematic = true;
    }

    void OnEnable()
    {
        // Listen for when the player grabs the object
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnBallGrabbed);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnBallGrabbed);
        }
    }

    private void OnBallGrabbed(SelectEnterEventArgs args)
    {
        // If this is the very first time it's being picked up
        if (!hasBeenPickedUp)
        {
            hasBeenPickedUp = true;
            rb.isKinematic = false; // Physics takes over permanently
        }
    }
}