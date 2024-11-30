using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;



public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float jumpForce = 0;
    private bool isGrounded = true;
    public float gravityMultiplier = 2.0f; // Increased multiplier for air control

    // UI for objective text
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetObjectiveText();
        StartCoroutine(ChangeObjectiveAfterDelay(5.0f));
    }

    void SetObjectiveText()
    {
        objectiveText.text = "You are hungry";
    }

    IEnumerator ChangeObjectiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        objectiveText.text = "Go find the honey!";
        yield return new WaitForSeconds(delay);
        objectiveText.text = "Objective: Find the Honey.";
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue value)
    {
        // Check if the jump input is pressed and the player is grounded
        if (isGrounded && value.isPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;  // Prevent multiple jumps until grounded again
        }
    }

    private void FixedUpdate()
    {
        // Get the camera's forward and right directions
        Camera camera = Camera.main; // Ensure you have a camera tagged as "MainCamera"
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        // Flatten the camera forward vector
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Calculate movement direction based on camera orientation
        Vector3 movement = (cameraRight * movementX + cameraForward * movementY).normalized;
        rb.AddForce(movement * speed);

        if (movement != Vector3.zero) // Check if there is any movement input
        {
            // Calculate the desired rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            // Enforce -90 degrees on the X-axis
            targetRotation = Quaternion.Euler(-90, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            // Interpolate the rotation for smoother transition
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed));
        }

        // Only apply custom gravity when the player is not grounded
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityMultiplier * Physics.gravity.y, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has collided with the ground to allow jumping again
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}