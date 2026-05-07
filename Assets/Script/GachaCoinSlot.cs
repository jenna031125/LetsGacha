using UnityEngine;

public class GachaCoinSlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GachaMachine gachaMachine;

    private void OnTriggerEnter(Collider other)
    {
        TryAcceptCoin(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryAcceptCoin(other);
    }

    private void TryAcceptCoin(Collider other)
    {
        if (gachaMachine == null)
        {
            Debug.LogWarning("[GachaCoinSlot] GachaMachine reference is not assigned.");
            return;
        }

        if (!gachaMachine.CanInsertCoin())
            return;

        GachaCoin coin = FindCoinFromCollider(other);

        if (coin == null)
        {
            Debug.Log("[GachaCoinSlot] Entered object is not a coin: " + other.gameObject.name);
            return;
        }

        GameObject coinObject = coin.gameObject;

        ReleaseCoinFromController(coinObject);
        Destroy(coinObject);

        gachaMachine.InsertCoin();

        Debug.Log("[GachaCoinSlot] Coin accepted: " + coinObject.name);
    }

    private GachaCoin FindCoinFromCollider(Collider other)
    {
        GachaCoin coin = other.GetComponent<GachaCoin>();

        if (coin != null)
            return coin;

        coin = other.GetComponentInParent<GachaCoin>();

        if (coin != null)
            return coin;

        if (other.attachedRigidbody != null)
        {
            coin = other.attachedRigidbody.GetComponent<GachaCoin>();

            if (coin != null)
                return coin;
        }

        return null;
    }

    private void ReleaseCoinFromController(GameObject coinObject)
    {
        VRControllerGrab[] controllerGrabs =
            Object.FindObjectsByType<VRControllerGrab>(FindObjectsSortMode.None);

        foreach (VRControllerGrab controllerGrab in controllerGrabs)
        {
            if (controllerGrab.currentHeld == null)
                continue;

            MonoBehaviour heldObject = controllerGrab.currentHeld as MonoBehaviour;

            if (heldObject == null)
                continue;

            if (heldObject.gameObject == coinObject)
            {
                controllerGrab.GrabGone(true, coinObject.transform);
                Debug.Log("[GachaCoinSlot] Coin released from controller.");
                return;
            }
        }
    }
}