using UnityEngine;

public class VRRig : MonoBehaviour
{
    [SerializeField] Color outlineColor;
    public static VRRig Instance { get; private set; }
    [field: SerializeField] public Transform head { get; private set; }
    [SerializeField] VRController[] controllers;
    BoxCollider bc;

    public enum ControllerHand
    {
        Left, Right
    };

    public enum ControllerButton
    {
        Trigger, Grip, Thumbstick, Primary, Secondary
    }

    private void Start()
    {
        Instance = this;
        controllers[0].Initialise(this);
        controllers[1].Initialise(this);

        bc = GetComponent<BoxCollider>(); 
    }

    private void Update()
    {
        UpdateCollider();
    }

    void UpdateCollider()
    {
        Vector3 headPos = head.localPosition;
        Vector3 colSize = new Vector3(0.1f, 0.1f, 0.1f);
        headPos.y /= 2;
        bc.center = headPos;
        colSize.y = head.localPosition.y;
        bc.size = colSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = outlineColor;
        Vector3 rightForward = transform.position + transform.right + transform.forward;
        Vector3 rightBack = transform.position + transform.right - transform.forward;
        Vector3 leftForward = transform.position - transform.right + transform.forward;
        Vector3 leftBack = transform.position - transform.right - transform.forward;
        Gizmos.DrawLine(rightForward, rightBack);
        Gizmos.DrawLine(leftForward, leftBack);
        Gizmos.DrawLine(rightForward, leftForward);
        Gizmos.DrawLine(rightBack, leftBack);
    }

    public VRController GetController(ControllerHand hand)
    {
        if(hand == ControllerHand.Left)
        {
            return controllers[0];
        }
        else
        {
            return controllers[1];
        }
    }
}
