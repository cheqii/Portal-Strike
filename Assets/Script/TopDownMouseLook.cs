using UnityEngine;

public class TopDownMouseLook : MonoBehaviour
{
    public float rotationSpeed = 5.0f; // Rotation speed of the character
    public LayerMask groundLayer; // Layer mask for the ground or surfaces to hit

    void Update()
    {
        // Cast a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Get the point where the ray hits the ground
            Vector3 targetPosition = hit.point;

            // Calculate the direction from the character to the mouse cursor
            Vector3 direction = targetPosition - transform.position;

            // Ensure the character doesn't tilt upwards or downwards
            direction.y = 0.0f;

            // If the direction is non-zero, rotate the character to look at the mouse cursor
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}