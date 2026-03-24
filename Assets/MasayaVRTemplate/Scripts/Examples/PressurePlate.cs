using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent eventTrigger;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Found");

        if (other.CompareTag("Player"))
        {
            eventTrigger.Invoke();
        }
    }
}
