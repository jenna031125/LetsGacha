using UnityEngine;

public class GachaHandleGrab : MonoBehaviour, IGrabbable
{
    [Header("References")]
    [SerializeField] private GachaMachine gachaMachine;
    [SerializeField] private Transform rotatingPart;

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 localRotationAxis = Vector3.forward;
    [SerializeField] private float requiredAngle = 90f;
    [SerializeField] private float rotationSensitivity = 1f;
    [SerializeField] private bool returnToStartOnRelease = true;
    [SerializeField] private float returnSpeed = 180f;

    private VRControllerGrab currentController;
    private Quaternion startLocalRotation;
    private Vector3 previousControllerDirection;
    private float currentAngle;
    private bool isGrabbed;
    private bool hasTriggered;

    private void Awake()
    {
        if (rotatingPart == null)
        {
            rotatingPart = transform;
        }

        startLocalRotation = rotatingPart.localRotation;
    }

    private void Update()
    {
        if (isGrabbed)
        {
            UpdateHandleRotation();
            CheckDispenseAngle();
        }
        else if (returnToStartOnRelease)
        {
            ReturnToStartRotation();
        }
    }

    public void GrabStart(VRControllerGrab controller)
    {
        if (controller == null)
            return;

        currentController = controller;
        isGrabbed = true;
        hasTriggered = false;

        previousControllerDirection = GetControllerDirection();

        Debug.Log("[GachaHandle] Grab started.");
    }

    public void GrabEnd()
    {
        Debug.Log("[GachaHandle] Grab ended.");

        if (currentController != null)
        {
            currentController.GrabGone(true, transform);
        }

        currentController = null;
        isGrabbed = false;
    }

    private void UpdateHandleRotation()
    {
        if (currentController == null)
            return;

        Vector3 currentDirection = GetControllerDirection();

        if (previousControllerDirection == Vector3.zero || currentDirection == Vector3.zero)
        {
            previousControllerDirection = currentDirection;
            return;
        }

        Vector3 worldAxis = rotatingPart.TransformDirection(localRotationAxis.normalized);

        float deltaAngle = Vector3.SignedAngle(
            previousControllerDirection,
            currentDirection,
            worldAxis
        );

        deltaAngle *= rotationSensitivity;

        currentAngle += deltaAngle;
        currentAngle = Mathf.Clamp(currentAngle, 0f, requiredAngle);

        rotatingPart.localRotation = startLocalRotation * Quaternion.AngleAxis(currentAngle, localRotationAxis.normalized);

        previousControllerDirection = currentDirection;
    }

    private Vector3 GetControllerDirection()
    {
        if (currentController == null)
            return Vector3.zero;

        Vector3 worldAxis = rotatingPart.TransformDirection(localRotationAxis.normalized);
        Vector3 direction = currentController.transform.position - rotatingPart.position;

        direction = Vector3.ProjectOnPlane(direction, worldAxis);

        if (direction.sqrMagnitude < 0.0001f)
            return Vector3.zero;

        return direction.normalized;
    }

    private void CheckDispenseAngle()
    {
        if (hasTriggered)
            return;

        if (currentAngle < requiredAngle)
            return;

        if (gachaMachine == null)
        {
            Debug.LogWarning("[GachaHandle] GachaMachine reference is not assigned.");
            return;
        }

        bool success = gachaMachine.TryDispense();

        if (success)
        {
            hasTriggered = true;
            Debug.Log("[GachaHandle] Dispense triggered.");
        }
    }

    private void ReturnToStartRotation()
    {
        if (rotatingPart == null)
            return;

        if (currentAngle <= 0f)
        {
            currentAngle = 0f;
            rotatingPart.localRotation = startLocalRotation;

            if (gachaMachine != null)
            {
                gachaMachine.ResetDispenseState();
            }

            return;
        }

        currentAngle = Mathf.MoveTowards(currentAngle, 0f, returnSpeed * Time.deltaTime);
        rotatingPart.localRotation = startLocalRotation * Quaternion.AngleAxis(currentAngle, localRotationAxis.normalized);
    }
}