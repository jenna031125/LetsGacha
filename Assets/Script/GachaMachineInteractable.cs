using System.Collections.Generic;
using UnityEngine;

public class GachaMachineInteractable : MonoBehaviour, IInteractable
{
    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;

    [Header("Optional")]
    [SerializeField] private bool useSpawnPointRotation = true;

    private VRControllerInteraction currentInteractionController;

    public void InteractStart(VRControllerInteraction controller)
    {
        currentInteractionController = controller;
        Debug.Log($"{gameObject.name} : InteractStart");
    }

    public void Interact()
    {
        if (currentInteractionController == null)
        {
            Debug.Log("No active interaction controller.");
            return;
        }

        VRController vrController = currentInteractionController.GetComponent<VRController>();
        if (vrController == null)
        {
            Debug.LogWarning("VRController not found.");
            return;
        }

        if (vrController.grab == null)
        {
            Debug.LogWarning("VRControllerGrab not found.");
            return;
        }

        IGrabbable heldItem = vrController.grab.currentHeld;
        if (heldItem == null)
        {
            Debug.Log("No object is currently held.");
            return;
        }

        MonoBehaviour heldObject = heldItem as MonoBehaviour;
        if (heldObject == null)
        {
            Debug.LogWarning("Failed to cast held object to MonoBehaviour.");
            return;
        }

        GachaCoin coin = heldObject.GetComponent<GachaCoin>();
        if (coin == null)
        {
            Debug.Log("The held object is not a coin.");
            return;
        }

        ConsumeHeldCoin(vrController, heldObject.gameObject);
        SpawnRandomItem();
    }

    public void InteractEnd()
    {
        currentInteractionController = null;
        Debug.Log($"{gameObject.name} : InteractEnd");
    }

    private void ConsumeHeldCoin(VRController vrController, GameObject coinObject)
    {
        Destroy(coinObject);

        // Clear reference from controller
        vrController.grab.GrabGone(false, null);

        Debug.Log("Coin consumed.");
    }

    private void SpawnRandomItem()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name} : itemPrefabs is empty.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning($"{gameObject.name} : spawnPoint is not assigned.");
            return;
        }

        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject selectedPrefab = itemPrefabs[randomIndex];

        if (selectedPrefab == null)
        {
            Debug.LogWarning($"{gameObject.name} : selected prefab is null.");
            return;
        }

        Quaternion rotation = useSpawnPointRotation
            ? spawnPoint.rotation
            : selectedPrefab.transform.rotation;

        Instantiate(selectedPrefab, spawnPoint.position, rotation);

        Debug.Log($"{gameObject.name} : Spawned {selectedPrefab.name}");
    }
}