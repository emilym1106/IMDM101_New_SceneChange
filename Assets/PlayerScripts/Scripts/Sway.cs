using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    // Amplitude of the sway (how far it goes)
    public float swayAmount = 0;
    // Speed of the sway
    public float swaySpeed = 0;

    // Original rotation of the object
    private Quaternion originalRotation;

    void Start()
    {
        // Store the original rotation to apply the sway around this point
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the sway angle using a sine wave
        float swayAngle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        
        // Apply the sway to the y-axis while keeping the original x and z rotations
        transform.rotation = originalRotation * Quaternion.Euler(0, swayAngle, 0);
    }
}