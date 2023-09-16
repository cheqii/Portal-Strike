using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow (usually the player)
    public float smoothSpeed = 5.0f; // Adjust the smoothness of the camera follow

    private Vector3 offset;

    private void Start()
    {
        // Calculate the initial offset between the camera and the target
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Calculate the desired camera position
        Vector3 targetPosition = target.position + offset;

        // Keep the y-axis position constant
        targetPosition.y = transform.position.y;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
