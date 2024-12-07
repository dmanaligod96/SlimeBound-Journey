using UnityEngine;

public class EnemyFlyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float followDistance = 5f; // Distance at which the enemy starts following
    public float moveSpeed = 2f; // Speed of movement
    public float patrolSpeed = 1f; // Speed for general movement (patrolling)

    private bool isFollowing = false; // Whether the enemy is following the player

    private Vector2 startPosition; // Starting position for patrolling
    private Vector2 targetPosition; // Target position for patrolling

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition; // Initially target the start position
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= followDistance)
        {
            // Start following the player
            isFollowing = true;
        }
        else
        {
            // Stop following the player
            isFollowing = false;
        }

        if (isFollowing)
        {
            FollowPlayer();
        }
        else
        {
            PatrolMovement();
        }
    }

    private void FollowPlayer()
    {
        // Move towards the player's position
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void PatrolMovement()
    {
        // Move in a general patrol direction on the X and Y axis
        targetPosition = new Vector2(startPosition.x + Mathf.Sin(Time.time) * 3f, startPosition.y + Mathf.Cos(Time.time) * 3f);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
    }
}
