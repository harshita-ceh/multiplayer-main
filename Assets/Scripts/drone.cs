using UnityEngine;

public class drone : MonoBehaviour
{
    public Transform centerPoint; // The center point of the circular path
    public float radius = 2f;     // The radius of the circular path
    public float speed = 30f;     // The speed of rotation in degrees per second

    private Vector3 originalPosition;

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogError("Center point not assigned. Please assign a center point in the Inspector.");
            enabled = false; // Disable the script if the center point is not assigned.
        }

        originalPosition = transform.position - centerPoint.position;
    }

    void Update()
    {
        // Calculate the new position in the circular path
        float angle = Time.time * speed;
        Vector3 newPosition = centerPoint.position + Quaternion.Euler(0, angle, 0) * originalPosition;
        transform.position = newPosition;
    }
}
