using System.Collections.Generic;
using UnityEngine;

public class GachaMachine : MonoBehaviour
{
    [Header("Item Spawn Settings")]
    [SerializeField] private List<GameObject> itemPrefabs = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool useSpawnPointRotation = true;

    [Header("Coin State")]
    [SerializeField] private bool requireCoin = true;
    [SerializeField] private bool coinInserted = false;

    private bool hasDispensedThisTurn = false;

    public bool CanInsertCoin()
    {
        return !coinInserted;
    }

    public void InsertCoin()
    {
        coinInserted = true;
        hasDispensedThisTurn = false;

        Debug.Log("[GachaMachine] Coin inserted.");
    }

    public bool TryDispense()
    {
        if (hasDispensedThisTurn)
        {
            Debug.Log("[GachaMachine] Dispense already completed for this turn.");
            return false;
        }

        if (requireCoin && !coinInserted)
        {
            Debug.Log("[GachaMachine] Cannot dispense. No coin inserted.");
            return false;
        }

        SpawnRandomItem();

        coinInserted = false;
        hasDispensedThisTurn = true;

        Debug.Log("[GachaMachine] Dispense completed.");

        return true;
    }

    public void ResetDispenseState()
    {
        hasDispensedThisTurn = false;
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