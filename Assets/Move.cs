using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Переміщення")]
    public float speed = 5f; // Шивдкість руху (дробове число)
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

        Vector3 move = Quaternion.Euler(0, yaw, 0) * new Vector3(horizontal, 0, vertical);
        transform.Translate(move * speed * Time.deltaTime, Space.World);

        playerCamera.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 1,
            transform.position.z
            );

        yaw += horizontal_M * mouseSens;
        pitch = Mathf.Clamp(pitch - vertical_M * mouseSens, -85f, 85f);
        playerCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
