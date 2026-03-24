using UnityEngine;

public interface IInteractable
{
    public void InteractStart(VRControllerInteraction controller);
    public void Interact();
    public void InteractEnd();
}
