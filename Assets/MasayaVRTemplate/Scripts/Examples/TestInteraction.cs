using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    public void InteractStart(VRControllerInteraction controller)
    {
        Debug.Log("Interacted");
        controller.InteractFinish(false);
    }
    public void Interact()
    {

    }
    public void InteractEnd()
    {

    }
}
