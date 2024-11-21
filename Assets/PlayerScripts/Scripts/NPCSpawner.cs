
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour

{
    public GameObject[] animalPrefabs; // Array of animal prefabs
    public int minAnimalCount = 10; // Minimum number of animal objects to spawn
    public int maxAnimalCount = 50; // Maximum number of animal objects to spawn
    public LayerMask groundLayer; // Layer mask for ground
    public float spawnRadius = 0.5f; // Minimum distance between animal objects

    private Collider groundCollider;

    void Start()
    {
        // Find the ground collider
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundCollider = ground.GetComponent<Collider>();
            if (groundCollider != null)
            {
                SpawnAnimal();
            }
            else
            {
                Debug.LogError("Ground object has no Collider attached!");
            }
        }
        else
        {
            Debug.LogError("No object tagged as 'Ground' found!");
        }
    }

    void SpawnAnimal()
    {
        if (animalPrefabs.Length == 0)
        {
            Debug.LogError("No animal prefabs assigned!");
            return;
        }

        int animalCount = Random.Range(minAnimalCount, maxAnimalCount);
        int spawned = 0;

        for (int i = 0; i < animalCount; i++)
        {
            Vector3 spawnPoint = GetRandomPointOnGround();

            if (spawnPoint != Vector3.zero)
            {
                // Check for overlaps
                if (!Physics.CheckSphere(spawnPoint, spawnRadius, ~groundLayer))
                {
                    GameObject randomAnimalPrefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];
                    Instantiate(randomAnimalPrefab, spawnPoint, Quaternion.identity);
                    spawned++;
                }
                else
                {
                    i--; // Retry if overlap is detected
                }
            }
        }

        Debug.Log($"Spawned {spawned} animal objects.");
    }

    Vector3 GetRandomPointOnGround()
    {
        // Generate a random point within the ground bounds
        Bounds bounds = groundCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        Vector3 randomPoint = new Vector3(randomX, bounds.center.y + 50f, randomZ);

        // Raycast to find the exact point on the ground
        if (Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return hit.point;
            }
        }

        return Vector3.zero; // Return zero vector if no valid point is found
    }
}