using UnityEngine;

public class HoppingEnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of movement
    public float hopHeight = 2f; // Height of the jump
    public float hopInterval = 2f; // Time between hops
    public Vector2 movementRange = new Vector2(5f, 5f); // Horizontal and vertical limits for hopping

    private Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component
    private Vector2 startPosition; // Starting position for hops
    private float timeSinceLastHop; // Timer to control hop intervals
    private bool isFacingRight = true; // To track the direction the enemy is facing

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        timeSinceLastHop = hopInterval; // Set the initial hop time
    }

    private void Update()
    {
        timeSinceLastHop += Time.deltaTime;

        // If enough time has passed, hop
        if (timeSinceLastHop >= hopInterval)
        {
            HopAround();
            timeSinceLastHop = 0f; // Reset the hop timer
        }
    }

    private void HopAround()
    {
        // Calculate the new position
        Vector2 targetPosition = new Vector2(
            Random.Range(startPosition.x - movementRange.x, startPosition.x + movementRange.x),
            Random.Range(startPosition.y - movementRange.y, startPosition.y + movementRange.y)
        );

        // Move the enemy toward the target position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Apply movement along the X-axis
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        // Apply jump (Y-axis movement)
        rb.velocity = new Vector2(rb.velocity.x, hopHeight);

        // Optionally, reset vertical velocity to simulate gravity after the jump
        // (can make the jump arc feel more natural)
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -10f, Mathf.Infinity));

        // Flip the sprite based on direction
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Flip the sprite horizontally by changing the sign of the localScale's x
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x = -scale.x; // Flip the sprite
        transform.localScale = scale;
    }
}
