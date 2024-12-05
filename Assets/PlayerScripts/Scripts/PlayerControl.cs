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
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private bool isGrounded = true;
    public float gravityMultiplier = 2.0f; // Increased multiplier for air control

    // UI for objective text
    public TextMeshProUGUI objectiveText;

    // Animator for handling animations
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();  // Make sure your Player GameObject has an Animator component
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

        // Trigger the appropriate animation based on movement
        bool isWalking = movementX != 0 || movementY != 0;
        animator.SetBool("isWalking", isWalking);
    }

    void OnJump(InputValue value)
    {
        if (isGrounded && value.isPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;  // Prevent multiple jumps until grounded again
            animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        Camera camera = Camera.main; // Ensure you have a camera tagged as "MainCamera"
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movement = (cameraRight * movementX + cameraForward * movementY).normalized;
        rb.AddForce(movement * speed);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed));
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityMultiplier * Physics.gravity.y, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}