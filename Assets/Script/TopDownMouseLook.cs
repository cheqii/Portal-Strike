using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMouseLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f; // Rotation speed of the character
    [SerializeField] private LayerMask groundLayer; // Layer mask for the ground or surfaces to hit

    // private Shooting shooting;
    [SerializeField] public Nf_GameEvent shootEvent;

    private PlayerInput playerInput;

    private void Awake()
    {
        // Get the PlayerInput component attached to the player GameObject
        playerInput = GetComponent<PlayerInput>();
        // Get the Shooting component script attached to the player GameObject
        // shooting = GetComponent<Shooting>();
    }

    private void Update()
    {
        HandleMouseInput();
        HandleStickInput();
    }

    private void RotateCharacter(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleMouseInput()
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
                RotateCharacter(direction);
                HandleMouseClick();
            }
        }
    }

    private void HandleStickInput()
    {
        // Cast a ray to right stick direction {input}
        Vector2 input = playerInput.actions["Look"].ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y);

        // If the {inputDirection} is non-zero, rotate the character to look at the stick direction {input}
        if (inputDirection != Vector3.zero)
        {
            RotateCharacter(inputDirection);
            // shooting.Perform();
            shootEvent.Raise();
        }
    }


    private void HandleMouseClick()
    {
        // Left Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            // shooting.Perform();
            shootEvent.Raise();
        }
    }
}