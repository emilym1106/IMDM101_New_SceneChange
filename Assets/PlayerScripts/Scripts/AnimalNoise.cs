using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalNoise : MonoBehaviour
{
    public AudioClip[] animalSounds;     // Array to hold different animal sounds
    public float minInterval = 5f;       // Minimum interval between sounds
    public float maxInterval = 15f;      // Maximum interval between sounds
    public Transform player;             // Reference to the player's transform
    public float maxDistance = 20f;      // Maximum distance at which the sound is audible
    public float minVolume = 0.1f;       // Minimum volume when the player is far away
    public float maxVolume = 1.0f;       // Maximum volume when the player is close

    private AudioSource audioSource;     // Reference to the AudioSource component
    private float nextSoundTime;         // Time for the next sound to play

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ScheduleNextSound();
    }

    void Update()
    {
        AdjustVolumeBasedOnDistance();

        if (Time.time >= nextSoundTime)
        {
            PlayRandomSound();
            ScheduleNextSound();
        }
    }

    void PlayRandomSound()
    {
        if (animalSounds.Length == 0) return; // Ensure there's at least one sound

        int randomIndex = Random.Range(0, animalSounds.Length);
        audioSource.PlayOneShot(animalSounds[randomIndex]);
    }

    void ScheduleNextSound()
    {
        nextSoundTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void AdjustVolumeBasedOnDistance()
    {
        if (player == null) return; // Ensure the player is assigned

        float distance = Vector3.Distance(transform.position, player.position);

        // Calculate the volume based on the distance (closer = louder)
        float volume = Mathf.Clamp(1 - (distance / maxDistance), minVolume, maxVolume);
        audioSource.volume = volume;
    }
}