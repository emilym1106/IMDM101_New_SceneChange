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
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
       
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

        // Apply continuous jumping if the space key is held down
        if (Input.GetKey(KeyCode.Space))
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
    count = count + 1;
    if(other.gameObject.CompareTag("Flower"))
    {
        other.gameObject.SetActive(false);
       
    }
   }
     

}