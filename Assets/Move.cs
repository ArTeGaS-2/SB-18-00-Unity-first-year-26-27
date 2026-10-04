using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject playerCamera;
    public float speed = 5f; // Шивдкість руху (дробове число)
    public float mouseSens = 5f; // Чутливість миші (теж float - дробова)

    Rigidbody rb; // Фізичний компонент

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(
            horizontal * speed * Time.deltaTime,
            0,
            vertical * speed * Time.deltaTime
            );

        playerCamera.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 1,
            transform.position.z
            );

        float horizontal_M = Input.GetAxis("Mouse X");
        float vertical_M = Input.GetAxis("Mouse Y");

        transform.Rotate(0, horizontal_M * mouseSens, 0);
        playerCamera.transform.Rotate(
            vertical_M * mouseSens,
            0,
            0);

    }
}
