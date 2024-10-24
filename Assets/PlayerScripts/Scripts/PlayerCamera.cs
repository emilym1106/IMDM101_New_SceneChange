using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    public float rotationSpeed = 0;
    public float smoothSpeed = 0;

    void Start()
    {
    offset = transform.position - player.transform.position;

        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButton(1)) // 1 refers to the right mouse button
        {
            // Get mouse movement input
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
            float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // Rotate the offset around the player based on mouse movement
            Quaternion camTurnAngle = Quaternion.AngleAxis(horizontal, Vector3.up);
            offset = camTurnAngle * offset;

            // Vertical rotation (up/down) to look up or down, clamped for better control
            Quaternion verticalRotation = Quaternion.AngleAxis(vertical, transform.right);
            offset = verticalRotation * offset;

            // Smoothly interpolate to the new camera position
        Vector3 desiredPosition = player.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed);

        // Smoothly rotate the camera to look at the player
        Quaternion desiredRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
        }

        transform.position = player.transform.position + offset;
    }
}
