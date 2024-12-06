using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private Slime playerSlime;

    void Update()
    {
        // Handle jumping input in Update for better responsiveness
        if (Input.GetButtonDown("Jump"))
        {
            playerSlime.Jump();
        }
    }

    void FixedUpdate()
    {
        // Handle movement input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        playerSlime.Move(movement);
    }
    
}
