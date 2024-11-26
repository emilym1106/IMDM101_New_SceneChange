using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player; // The player GameObject
    private Vector3 offset;   // Offset from the player
    public float rotationSpeed = 5f; // Speed of camera rotation
    public float smoothSpeed = 0.125f; // Smooth transition speed

    public float zoomSpeed = 2f; // Speed of zooming
    public float maxZoom = 20f;  // Maximum zoom distance
    public float minZoom = 5f;   // Minimum zoom distance
    private float currentZoom;   // Current zoom distance

    void Start()
    {
        offset = transform.position - player.transform.position;
        currentZoom = offset.magnitude; // Set the initial zoom distance
    }

    void LateUpdate()
    {
        // Handle rotation with the right mouse button
        if (Input.GetMouseButton(1)) // 1 refers to the right mouse button
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed;

            Quaternion camTurnAngle = Quaternion.AngleAxis(horizontal, Vector3.up);
            offset = camTurnAngle * offset;

            Quaternion verticalRotation = Quaternion.AngleAxis(vertical, transform.right);
            offset = verticalRotation * offset;
        }

        // Handle zooming with the mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // Update offset length based on zoom
        offset = offset.normalized * currentZoom;

        // Smoothly position and rotate the camera
        Vector3 desiredPosition = player.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed);

        Quaternion desiredRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
    }
}