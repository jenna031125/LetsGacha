using UnityEngine;

public class TestGrab : MonoBehaviour, IGrabbable
{
    VRControllerGrab currentController;
    Rigidbody rb;

    public bool IsGrabbed => currentController != null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GrabStart(VRControllerGrab controller)
    {
        Debug.Log("Grab Start");

        if (currentController != null)
        {
            if (currentController != controller)
            {
                currentController.GrabEnd();
                currentController = controller;
                ParentObject();
            }
        }
        else
        {
            currentController = controller;
            ParentObject();
        }
    }

    void ParentObject()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.SetParent(currentController.transform, true);
    }

    public void GrabEnd()
    {
        transform.SetParent(null, true);

        currentController.GrabGone(true, transform);

        currentController = null;

        rb.isKinematic = false;
        rb.useGravity = true;
    }
}