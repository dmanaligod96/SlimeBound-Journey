using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Sprite checkpointActiveSprite; // The sprite to display when checkpoint is active
    [SerializeField] private Sprite checkpointDefaultSprite; // The default sprite before the checkpoint is activated
    [SerializeField] static bool checkpointSet = false;
    private static Vector3 checkpointPosition;
    

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Set the default sprite initially
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = checkpointDefaultSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Set checkpoint position when the player hits the checkpoint
            checkpointPosition = transform.position;
            checkpointSet = true;

            // Change the sprite when the checkpoint is set
            if (spriteRenderer != null && checkpointSet)
            {
                spriteRenderer.sprite = checkpointActiveSprite;
            }
        }
    }

    public static void RespawnPlayer(GameObject player)
    {
        if (checkpointSet)
        {
            player.transform.position = checkpointPosition;
        }
    }
}
