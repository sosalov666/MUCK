using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;

    [Header("Ground Check")]
    [SerializeField] private bool isGround = true;

    [Header("Camera Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float verticalClamp = 80f;

    [SerializeField] private Transform playerCamera; // ������ ������ ���� ��������� ��������

    private float verticalRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // ��������� ������ ����
    }

    private void Update()
    {
        // ��������� ������
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isGround = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ������ � �������������� ������
        }

        // ���������� �������
        HandleCameraRotation();
    }

    private void FixedUpdate()
    {
        // ���������� ���������
        HandleMovement();
    }

    private void HandleMovement()
    {
        // ��������� ������� ������ ��� ��������
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // �������� ������������ ����������� ������
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;

        // ����������� ������ � �������������� Rigidbody.MovePosition ��� �������� �����������
        Vector3 newPosition = rb.position + move * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void HandleCameraRotation()
    {
        // ��������� ������� ������ ��� �������� ������
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // �������� ������ �� ���������
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

        // ��������� ������� �� ��������� ������ � ������
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // �������� ��������� �� �����������
        transform.Rotate(Vector3.up * mouseX);
    }

    // ��������, ��� �������� �������� �����
    void OnCollisionEnter(Collision col)
    {
        isGround = true;

    }
}