using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1f;              // Standard movement speed
    public float speedBurstMultiplier = 2f;   // Multiplier for speed burst
    public float burstDuration = 1f;          // Duration of the speed burst
    public float moveRange = 5f;              // Maximum range from the starting position

    private Vector2 startingPosition;         // To store the initial position of the enemy
    private bool isMovingRight = true;        // Direction of movement
    private float burstTimer = 0f;            // Timer for burst duration
    private bool isInBurst = false;           // Check if the enemy is in a speed burst

    public float changeDirectionIntervalMin = 1f; // Minimum interval before changing direction
    public float changeDirectionIntervalMax = 3f; // Maximum interval before changing direction

    private void Start()
    {
        startingPosition = transform.position; // Store the starting position
        InvokeRepeating("RandomizeMovement", GetRandomInterval(), GetRandomInterval());
    }

    private void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        // Reset speed if burst duration has ended
        if (isInBurst)
        {
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0f)
            {
                isInBurst = false;
                moveSpeed /= speedBurstMultiplier; // Reset to normal speed
            }
        }

        // Check if the enemy is within the allowed range and change direction if it reaches the boundary
        if (Vector2.Distance(startingPosition, transform.position) >= moveRange)
        {
            isMovingRight = !isMovingRight;
        }

        // Move the enemy left or right at a constant speed
        float moveDirection = isMovingRight ? 1f : -1f;
        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);
    }

    void RandomizeMovement()
    {
        // Randomly trigger a speed burst without stacking the speed
        if (!isInBurst && Random.value < 0.05f) // 5% chance to activate speed burst
        {
            ActivateSpeedBurst();
        }

        // Schedule the next change of direction
        Invoke("RandomizeMovement", GetRandomInterval());
    }

    void ActivateSpeedBurst()
    {
        moveSpeed *= speedBurstMultiplier; // Increase speed for burst
        isInBurst = true;
        burstTimer = burstDuration; // Set the burst timer
    }

    float GetRandomInterval()
    {
        return Random.Range(changeDirectionIntervalMin, changeDirectionIntervalMax);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            // Define behavior when the enemy collides with a hazard (e.g., reverse direction)
            isMovingRight = !isMovingRight;
        }
    }
}