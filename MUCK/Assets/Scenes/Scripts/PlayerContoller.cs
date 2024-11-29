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

    [SerializeField] private Transform playerCamera; // Камера должна быть отдельным объектом

    private float verticalRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор мыши
    }

    private void Update()
    {
        // Обработка прыжка
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isGround = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Прыжок с использованием физики
        }

        // Управление камерой
        HandleCameraRotation();
    }

    private void FixedUpdate()
    {
        // Управление движением
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Получение входных данных для движения
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Движение относительно направления игрока
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed;

        // Перемещение игрока с использованием Rigidbody.MovePosition для плавного перемещения
        Vector3 newPosition = rb.position + move * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void HandleCameraRotation()
    {
        // Получение входных данных для поворота камеры
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Вращение камеры по вертикали
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);

        // Применяем поворот по вертикали только к камере
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Вращение персонажа по горизонтали
        transform.Rotate(Vector3.up * mouseX);
    }

    // Проверка, что персонаж касается земли
    void OnCollisionEnter(Collision col)
    {
        isGround = true;

    }
}