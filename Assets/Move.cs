using UnityEngine;

public class Move : MonoBehaviour
{
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

        rb.AddForce(
            horizontal, // X
            0,
            vertical,
            ForceMode.Force
            );
    }
}
