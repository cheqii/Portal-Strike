using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform target; // The target to follow (usually the player)
    public float smoothSpeed = 5.0f; // Adjust the smoothness of the camera follow
    public Vector3 offset;
    private bool startFollow = false;
    
    
    
    public void DoFollow()
    {
        offset = transform.position - target.position;
        startFollow = true;
    }

    private void LateUpdate()
    {
        if (startFollow == false)
        {
            return;
        }
        
        // Calculate the desired camera position
        Vector3 targetPosition = target.position + offset;

        // Keep the y-axis position constant
        targetPosition.y = transform.position.y;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}