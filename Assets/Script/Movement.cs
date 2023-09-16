using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust the player's movement speed as needed

    private CharacterController characterController;

    private void Start()
    {
        // Get the CharacterController component attached to the player GameObject
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput);

        // Normalize the direction vector to avoid faster diagonal movement
        if (movementDirection.magnitude > 1)
        {
            movementDirection.Normalize();
        }

        // Calculate the final movement vector
        Vector3 movement = movementDirection * moveSpeed * Time.deltaTime;

        // Apply the movement to the player using CharacterController
        characterController.Move(movement);
    }
}