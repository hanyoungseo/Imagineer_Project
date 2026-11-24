
using UnityEngine;

public class CardModelController : MonoBehaviour
{
    [Header("Tracking Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float positionThreshold = 0.005f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isTracking = true;

    private void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (!isTracking)
            return;

        float distance = Vector3.Distance(
            transform.position, targetPosition);

        if (distance > positionThreshold)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                1f - Mathf.Exp(-moveSpeed * Time.deltaTime)
            );
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
        );
    }

    public void SetTargetPose(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

    public void SetTracking(bool state)
    {
        isTracking = state;
    }

    public void ResetPose(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;

        transform.SetPositionAndRotation(position, rotation);
    }
}
