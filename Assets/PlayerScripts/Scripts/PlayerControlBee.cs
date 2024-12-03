using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerContolBee : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float jumpForce = 0;
    private bool isGrounded = true;
    public float gravityMultiplier = 0;
    public float maxHeight = 10.0f; // Set the maximum height limit
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetObjectiveText();
        StartCoroutine(ChangeObjectiveAfterDelay(5.0f));
    }

    void SetObjectiveText()
    {
        objectiveText.text = "Looks like your pot is empty";
    }

    IEnumerator ChangeObjectiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        objectiveText.text = "Go find and collect at least 5 red flowers";
        yield return new WaitForSeconds(delay);
        StartCoroutine(UpdateObjectiveText());
    }

    IEnumerator UpdateObjectiveText()
    {
        while (count < 5) // Continue updating until 5 flowers are collected
        {
            objectiveText.text = $"Objective: Find 5 Red Flowers.\n{count} Flowers Collected";
            yield return null; // Wait until the next frame
        }

        // Objective complete message once 5 flowers are collected
        objectiveText.text = "You collected 5 red flowers! Now return to your pot!";
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
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
            // Adjust the target rotation to keep a fixed Y rotation of -90
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y - 90, targetRotation.eulerAngles.z);

            // Interpolate the rotation for smoother transition
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed));
        }

        // Apply continuous jumping if the space key is held down and height is within the limit
        if (Input.GetKey(KeyCode.Space) && transform.position.y < maxHeight)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityMultiplier * Physics.gravity.y, ForceMode.Acceleration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flower"))
        {
            other.gameObject.SetActive(false);
            count++;
            Debug.Log("Flowers collected: " + count);
        }
    }

    public int GetFlowerCount()
    {
        return count;
    }
}