using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions controls; // Reference to the input actions
    private Vector2 moveInput; // The movement input (from A/D keys)
    private Rigidbody rb; // Reference to the Rigidbody
    private float moveSpeed = 5f; // The movement speed

    [SerializeField] private float maxLeft = -5f; // Left boundary
    [SerializeField] private float maxRight = 5f; // Right boundary

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody attached to the player
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // Get the input value for horizontal movement (A/D keys or joystick)
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Clamp the player's position to stay within boundaries
        Vector3 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, maxLeft, maxRight);

        // Apply the clamped position
        rb.position = clampedPosition;

        // Set the Rigidbody's velocity based on movement input
        float velocityX = moveInput.x * moveSpeed;

        // If at the left boundary, prevent leftward movement (can move right)
        if (clampedPosition.x == maxLeft && moveInput.x < 0)
        {
            velocityX = 0;
        }
        // If at the right boundary, prevent rightward movement (can move left)
        else if (clampedPosition.x == maxRight && moveInput.x > 0)
        {
            velocityX = 0;
        }

        // Update the Rigidbody's velocity (preserving the Y and Z axis velocity)
        Vector3 velocity = new Vector3(velocityX, rb.linearVelocity.y, rb.linearVelocity.z);
        rb.linearVelocity = velocity;
    }



}
