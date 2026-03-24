using UnityEngine;
using UnityEngine.Events;

public class EventPlay : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent vrEvent;

    public void Interact()
    {
        
    }

    public void InteractEnd()
    {
        
    }

    public void InteractStart(VRControllerInteraction controller)
    {
        vrEvent.Invoke();
    }
}
