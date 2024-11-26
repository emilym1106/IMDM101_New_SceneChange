using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGen : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Array of possible object prefabs
    public float spawnYPosition = 20f; // Fixed Y position for objects
    public float spawnXRange = 10f;    // Range for random X position
    public float spawnZRange = 10f;    // Range for random Z position
    public float spawnInterval = 2f;  // Interval in seconds between spawns
    public float objectSpeed = 2f;    // Speed at which objects move left
    public float despawnXPosition = -15f; // X position at which objects are destroyed
    public int initialObjectCount = 10;   // Number of objects to spawn initially

    private void Start()
    {
        // Spawn initial objects
        GenerateInitialObjects();

        // Start the periodic object generation
        InvokeRepeating(nameof(GenerateObject), 0f, spawnInterval);
    }

    private void GenerateInitialObjects()
    {
        for (int i = 0; i < initialObjectCount; i++)
        {
            GenerateObject(true); // Spawn objects randomly across the area
        }
    }

    private void GenerateObject(bool isInitial = false)
    {
        if (objectPrefabs.Length == 0)
        {
            Debug.LogWarning("No object prefabs assigned to the generator!");
            return;
        }

        // Choose a random prefab from the array
        GameObject randomPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        // Randomize the X and Z positions within the specified range
        float randomX = Random.Range(-spawnXRange, spawnXRange);
        float randomZ = Random.Range(-spawnZRange, spawnZRange);

        // Adjust X position for periodic spawns to come from the right
        if (!isInitial)
        {
            randomX = spawnXRange;
        }

        // Spawn the object at the random position
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, randomZ);
        GameObject spawnedObject = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);

        // Attach a script to move and despawn the object
        spawnedObject.AddComponent<MoveAndDespawn>().Initialize(objectSpeed, despawnXPosition);
    }
}

public class MoveAndDespawn : MonoBehaviour
{
    private float speed;
    private float despawnX;

    // Initialize with movement speed and despawn position
    public void Initialize(float movementSpeed, float despawnPosition)
    {
        speed = movementSpeed;
        despawnX = despawnPosition;
    }

    private void Update()
    {
        // Move the object to the left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Despawn the object if it moves past the despawn position
        if (transform.position.x <= despawnX)
        {
            Destroy(gameObject);
        }
    }
}