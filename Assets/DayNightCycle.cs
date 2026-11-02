using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float timeSpeed = 1f;

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(timeSpeed,0,0);
    }
}
