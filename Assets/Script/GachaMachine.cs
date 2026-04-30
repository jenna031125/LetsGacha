using System.Collections.Generic;
using UnityEngine;

public class GachaMachine : MonoBehaviour
{
    [Header("Item Spawn Settings")]
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool useSpawnPointRotation = true;

    [Header("Dispense Settings")]
    [SerializeField] private bool requireCoin = true;

    private bool hasDispensedThisTurn = false;

    public bool TryDispense(VRControllerGrab handleControllerGrab)
    {
        if (hasDispensedThisTurn)
        {
            Debug.Log("[GachaMachine] Dispense already completed for this handle turn.");
            return false;
        }

        if (handleControllerGrab == null)
        {
            Debug.LogWarning("[GachaMachine] Handle controller grab reference is null.");
            return false;
        }

        VRController handleController = handleControllerGrab.GetComponent<VRController>();

        if (handleController == null)
        {
            Debug.LogWarning("[GachaMachine] VRController was not found on the handle controller.");
            return false;
        }

        if (requireCoin)
        {
            bool coinConsumed = TryConsumeCoinFromControllers(handleController);

            if (!coinConsumed)
            {
                Debug.Log("[GachaMachine] No coin was found in either controller.");
                return false;
            }
        }

        SpawnRandomItem();
        hasDispensedThisTurn = true;

        return true;
    }

    public void ResetDispenseState()
    {
        hasDispensedThisTurn = false;
        // Debug.Log("[GachaMachine] Dispense state has been reset.");
    }

    private bool TryConsumeCoinFromControllers(VRController baseController)
    {
        if (TryConsumeCoinFromController(baseController))
            return true;

        if (baseController.otherController != null)
        {
            if (TryConsumeCoinFromController(baseController.otherController))
                return true;
        }

        return false;
    }

    private bool TryConsumeCoinFromController(VRController controller)
    {
        if (controller == null || controller.grab == null)
            return false;

        IGrabbable heldItem = controller.grab.currentHeld;

        if (heldItem == null)
            return false;

        MonoBehaviour heldObject = heldItem as MonoBehaviour;

        if (heldObject == null)
            return false;

        GachaCoin coin = heldObject.GetComponent<GachaCoin>();

        if (coin == null)
            return false;

        GameObject coinObject = heldObject.gameObject;

        controller.grab.GrabGone(false, null);
        Destroy(coinObject);

        Debug.Log("[GachaMachine] Coin consumed from controller: " + controller.GetHand());

        return true;
    }

    private void SpawnRandomItem()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogWarning("[GachaMachine] Item prefab list is empty.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("[GachaMachine] Spawn point is not assigned.");
            return;
        }

        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject selectedPrefab = itemPrefabs[randomIndex];

        if (selectedPrefab == null)
        {
            Debug.LogWarning("[GachaMachine] Selected item prefab is null.");
            return;
        }

        Quaternion spawnRotation = useSpawnPointRotation
            ? spawnPoint.rotation
            : selectedPrefab.transform.rotation;

        Instantiate(selectedPrefab, spawnPoint.position, spawnRotation);

        Debug.Log("[GachaMachine] Spawned item: " + selectedPrefab.name);
    }
}