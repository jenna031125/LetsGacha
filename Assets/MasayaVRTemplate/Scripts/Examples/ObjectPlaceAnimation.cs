using UnityEngine;
using UnityEngine.Events;

public class ObjectPlaceAnimation : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public UnityEvent eventTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == obj)
        {
            eventTrigger.Invoke();
            other.GetComponent<Collider>().isTrigger = true;
            other.GetComponent<IGrabbable>().GrabEnd();
            other.transform.position = transform.position;
        }
    }
}
