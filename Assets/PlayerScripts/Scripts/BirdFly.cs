using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public float speed = 5f;          // Speed of the bird's movement
    public float changeDirectionTime = 3f; // Time interval to change direction randomly
    public float heightPosition = 25f;
    private Vector3 moveDirection;    // Current direction of movement
    private float timer;              // Timer to track direction change

    void Start()
    {
        // Set initial direction
        ChangeDirection();
    }

    void Update()
    {
        // Keep the bird's y position locked at 20
        Vector3 position = transform.position;
        position.y = heightPosition;

        // Move the bird in the current direction
        position += moveDirection * speed * Time.deltaTime;

        // Apply the new position
        transform.position = position;

        // Rotate the bird to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Update the timer and check if it's time to change direction
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0f;
        }
    }

    // Change the bird's direction randomly
    private void ChangeDirection()
    {
        // Generate a random direction in the x-z plane
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, 0f, randomZ).normalized;
    }

    // Optional: Draw gizmos to visualize the locked height
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(transform.position.x, 20f, transform.position.z), new Vector3(transform.position.x, 20f + 2f, transform.position.z));
    }
}
