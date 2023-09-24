using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjust the player's movement speed as needed

    private CharacterController characterController;
    private PlayerInput playerInput;
    private InputActionMap actionMap;

    private void Awake()
    {
        // Get the CharacterController component attached to the player GameObject
        characterController = GetComponent<CharacterController>();

        // Get the PlayerInput component attached to the player GameObject
        playerInput = GetComponent<PlayerInput>();

        // Get the ("Player") Action Maps from the InputActions component script that attached with PlayerInput
        actionMap = playerInput.actions.FindActionMap("Player");
    }

    /* OnEnable and OnDisable are used to reduce script execution.
     * OnEnable will be started before "Start()", so you need to use "Awake()" instead of "Start()" function */
    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }


    private void Update()
    {
        // Get input from the player
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        // Calculate the movement direction
        Vector3 movementDirection = new Vector3(input.x, 0.0f, input.y);

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