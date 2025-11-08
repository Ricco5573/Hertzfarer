using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.5f;

    [Header("Look Settings")]
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float mouseSensitivity = 100f;

    [Header("Control Toggle")]
    [SerializeField] private bool controlsEnabled = true;

    private CharacterController controller;
    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor for FPS-style control
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!controlsEnabled)
            return; // Stop all movement and look

        HandleMovement();
        HandleLook();

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.green);
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 10f))
            {
                if (hit.collider.CompareTag("Interactible"))
                {

                    hit.collider.GetComponent<Interactable>().Interact();
                }
            }
        }
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // You can call this from another script or trigger
    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;

        if (!enabled)
        {
            // Optionally unlock cursor when controls are off
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


}
