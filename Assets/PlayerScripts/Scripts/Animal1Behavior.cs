using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal1Behavior : MonoBehaviour
{
    public float moveRadius = 10f;        // Radius within which NPC will move
    public float waitTime = 2f;           // Time NPC waits before moving again
    public float avoidanceRadius = 3f;    // Radius to avoid other NPCs
    public LayerMask obstacleLayer;       // Layer for obstacles
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 targetPosition;
    private float timer;

    public Animator animator;             // Reference to the Animator component

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // If animator is not assigned, try to get it automatically
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        MoveToRandomPosition();
    }

    void Update()
    {
        // Update the Animator's Speed parameter based on the agent's velocity
        if (agent != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        if (IsAgentOnNavMesh() && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;

            // Wait for the specified time, then choose a new position
            if (timer >= waitTime)
            {
                MoveToRandomPosition();
                timer = 0f;
            }
        }
    }

    void MoveToRandomPosition()
    {
        bool positionFound = false;

        // Try finding a valid position
        for (int i = 0; i < 10; i++) // Limit attempts to avoid endless loop
        {
            Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
            randomDirection += transform.position;

            // Check if the point is on the NavMesh
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out UnityEngine.AI.NavMeshHit hit, moveRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                targetPosition = hit.position;

                // Check for other NPCs in the avoidance radius
                Collider[] nearbyNPCs = Physics.OverlapSphere(targetPosition, avoidanceRadius);
                bool hasNearbyNPC = false;

                foreach (Collider collider in nearbyNPCs)
                {
                    if (collider != null && collider.gameObject != this.gameObject && collider.CompareTag("Animal")) // Ensure it's another NPC
                    {
                        hasNearbyNPC = true;
                        break;
                    }
                }

                // Check if there's an obstacle in the path
                Vector3 directionToTarget = targetPosition - transform.position;
                float distanceToTarget = directionToTarget.magnitude;
                bool hasObstacle = Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer);

                // If no NPC is nearby and no obstacle, we found a valid position
                if (!hasNearbyNPC && !hasObstacle)
                {
                    positionFound = true;
                    break;
                }
            }
        }

        // If a valid position was found, set it as the destination
        if (positionFound && IsAgentOnNavMesh())
        {
            agent.SetDestination(targetPosition);
        }
    }

    private bool IsAgentOnNavMesh()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            // Optional: log the issue only in debug mode
            Debug.LogWarning("Agent is not active or not on a NavMesh.", this);
            return false;
        }
        return true;
    }
}