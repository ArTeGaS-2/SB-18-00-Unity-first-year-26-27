using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Переміщення")]
    public float speed = 5f; // Шивдкість руху (дробове число)
    public float jumpForce = 5f; // Сила стрибка
    public GameObject respawnPoint;
    private bool onGround = true; // Чи на землі персонаж
    [Header("Камера")]
    public GameObject playerCamera;
    public float mouseSens = 5f; // Чутливість миші (теж float - дробова)
    public Vector3 cameraPos = Vector3.zero;

    Rigidbody rb; // Фізичний компонент
    float yaw, pitch; // yaw - право/ліво, pitch - вгору/вниз

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float horizontal_M = Input.GetAxis("Mouse X");
        float vertical_M = Input.GetAxis("Mouse Y");

        PlayerJump();

        Vector3 move = Quaternion.Euler(0, yaw, 0) * new Vector3(horizontal, 0, vertical);
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        playerCamera.transform.position = new Vector3(
            transform.position.x + cameraPos.x,
            transform.position.y + cameraPos.y,
            transform.position.z + cameraPos.z
            );

        yaw += horizontal_M * mouseSens;
        pitch = Mathf.Clamp(pitch - vertical_M * mouseSens, -85f, 85f);
        playerCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
    private void FixedUpdate()
    {
        if (transform.position.y <= -20) // Мінімальна висота, після якої персонаж повертається на старт
        {
            transform.position = respawnPoint.transform.position;
        }
    }
    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            transform.Translate(Vector3.up * jumpForce * Time.deltaTime, Space.World);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}
