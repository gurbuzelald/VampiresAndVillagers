using UnityEngine;

public class PlayerController : BaseCharacter
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Transform cameraTransform;
    public float lookSpeed = 2f;

    private Rigidbody _rigidbody;

    [SerializeField] LayerMask layerMask;

    private EnergyComponent energyComponent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        energyComponent = GetComponent<EnergyComponent>();
    }

    private void Update()
    {
        HandleCameraRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJump();
        }
        if (Input.GetKey(KeyCode.LeftShift) && !energyComponent.isEnergyZero)
        {
            HandleMovement(moveSpeed * 5);

            energyComponent.DecreseEnergy(1);
        }
        else
        {
            HandleMovement(moveSpeed);
        }
    }

    private void HandleMovement(float moveSpeed)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            energyComponent.IncreaseEnergy(1);            
        }

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);

        if (moveDirection.magnitude > 0.1f)
        {
            moveDirection.Normalize();

            moveDirection = cameraTransform.forward * moveDirection.z + cameraTransform.right * moveDirection.x;
            moveDirection.y = 0f;

            if (!IsGrounded())
            {
                transform.Translate(moveDirection * moveSpeed / 3 * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }


    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * lookSpeed);

        cameraTransform.Rotate(Vector3.left, mouseY * lookSpeed);
    }

    private void HandleJump()
    {
        if (IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up.normalized * jumpForce * Time.deltaTime * 100, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, layerMask);
    }
}
