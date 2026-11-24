
using UnityEngine;

public class CardModelController : MonoBehaviour
{
    [Header("Position Tracking")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotationSpeed = 6f;

    [Header("Tracking Threshold")]
    [SerializeField] private float positionThreshold = 0.003f;
    [SerializeField] private float rotationThreshold = 0.5f;

    [Header("Tracking Correction")]
    [SerializeField] private float teleportDistance = 1.5f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private bool isTracking = true;

    private void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (!isTracking)
            return;

        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        float distance = Vector3.Distance(
            transform.position, targetPosition);

        if (distance > teleportDistance)
        {
            transform.position = targetPosition;
            return;
        }

        if (distance <= positionThreshold)
            return;

        float smoothing =
            1f - Mathf.Exp(-moveSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothing
        );
    }

    private void UpdateRotation()
    {
        float angle = Quaternion.Angle(
            transform.rotation, targetRotation);

        if (angle <= rotationThreshold)
            return;

        float smoothing =
            1f - Mathf.Exp(-rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            smoothing
        );
    }

    public void SetTargetPose(
        Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

    public void SetTracking(bool state)
    {
        isTracking = state;
    }

    public void ResetPose(
        Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;

        transform.SetPositionAndRotation(
            position, rotation);
    }

    public float GetTrackingError()
    {
        return Vector3.Distance(
            transform.position, targetPosition);
    }
}
