using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class walksound : MonoBehaviour
{
    public AudioClip walkingSound;       // The sound to play when the animal is walking
    public float volume = 0.5f;          // Volume of the walking sound
    public float movementThreshold = 0.1f; // Threshold to detect movement
    public float checkInterval = 0.1f;   // Interval to check if the animal is moving

    private AudioSource audioSource;     // Reference to the AudioSource component
    private Vector3 lastPosition;        // Last recorded position
    private bool isMoving = false;       // Tracks if the animal is moving

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = walkingSound;
        audioSource.loop = true; // Set the walking sound to loop
        audioSource.volume = volume;

        lastPosition = transform.position;
        InvokeRepeating(nameof(CheckMovement), 0f, checkInterval);
    }

    void CheckMovement()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > movementThreshold)
        {
            if (!isMoving)
            {
                StartWalkingSound();
            }
        }
        else
        {
            if (isMoving)
            {
                StopWalkingSound();
            }
        }

        lastPosition = transform.position;
    }

    void StartWalkingSound()
    {
        isMoving = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void StopWalkingSound()
    {
        isMoving = false;
        audioSource.Stop();
    }
}