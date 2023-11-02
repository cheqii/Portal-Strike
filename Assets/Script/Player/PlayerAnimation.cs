using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    
    private PlayerInput playerInput;
    private Transform cam;
    private Vector3 camForward;
    private Vector3 move;
    private Vector3 moveInput;

    private float forwardAmount;
    private float turnAmount;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        playerInput = transform.parent.GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        if (cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = input.x * camForward + input.y * cam.right;
            
        }
        else
        {
            move = input.x * Vector3.forward + input.y * Vector3.right;

           
        }
        
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        Move(move);
    }

    public void ShootAnim()
    {
        _animator.SetTrigger("Shoot");
    }

    private void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        _animator.SetFloat("Turn",forwardAmount,0.1f,Time.deltaTime);
        _animator.SetFloat("Forward",turnAmount,0.1f,Time.deltaTime);

    }

    public void SetAnim(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    private void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;

        forwardAmount = localMove.z;
    }
}