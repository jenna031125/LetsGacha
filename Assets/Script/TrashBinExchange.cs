using UnityEngine;

public class TrashBinExchange : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        TryExchange(other);
    }

    private void TryExchange(Collider other)
    {
        GachaItem gachaItem = other.GetComponentInParent<GachaItem>();

        if (gachaItem == null)
        {
            Debug.Log("[TrashBin] Entered object is not a gacha item: " + other.gameObject.name);
            return;
        }

        GameObject itemObject = gachaItem.gameObject;

        TestGrab grab = itemObject.GetComponent<TestGrab>();

        if (grab != null && grab.IsGrabbed)
        {
            Debug.Log("[TrashBin] Item is still being held. Exchange cancelled.");
            return;
        }

        if (coinPrefab == null)
        {
            Debug.LogWarning("[TrashBin] Coin prefab is not assigned.");
            return;
        }

        if (coinSpawnPoint == null)
        {
            Debug.LogWarning("[TrashBin] Coin spawn point is not assigned.");
            return;
        }

        Destroy(itemObject);
        Instantiate(coinPrefab, coinSpawnPoint.position, coinSpawnPoint.rotation);

        Debug.Log("[TrashBin] Gacha item exchanged for a coin.");
    }
}