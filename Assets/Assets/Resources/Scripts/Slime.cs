using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Slime : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Movement")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] int maxJumps = 2;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] int currentJumps = 0;
    private bool isGrounded = false;
    private bool isJumping = false;

    [Header("Player Info")]
    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth;
    [SerializeField] Image[] heartContainers;
    [Header("Lives")]
    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;

    [Header("Sprite Reference")]
    [SerializeField] Sprite idle;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] Sprite upSprite;

    [Header("KnockBack")]
    [SerializeField] float pushBackForceHorizontal = 10f;
    [SerializeField] float pushBackForceVerticle = 10f;
    [SerializeField] private float invincibilityDuration = 1f;

    public PlayerAudioHandler audioHandler;
    public CoinManager cm;
    public LivesManager lm;
    

    private Vector3 initialSpawnPosiiton;
    private bool isInvincible = false;
    private bool isDamage = false;

    private float delay = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        initialSpawnPosiiton = transform.position;

        
    }

    void Start(){

        currentHealth = maxHealth;
        currentLives = maxLives;
        lm.livesCount = currentLives;
    
    }
    void Update()
    {
        if(rb.velocity.y < -0.1f){
            isJumping = false;
        }
    }

    public void Move(Vector2 movement)
    {
        if(isDamage) return;
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        UpdateSprite(movement);
    }

    public void Jump()
    {
        if (isGrounded || currentJumps < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumps++;
            isJumping = true;
            audioHandler.PlayJumpSound();
        }
    }

    private void UpdateSprite(Vector2 movement)
    {
        if (movement.x != 0)
        {
            sr.sprite = movement.x > 0 ? rightSprite : leftSprite;
            audioHandler.PlayStartWalkSound();
        }
        else if (isJumping || movement.y > 0.1f)
        {
            sr.sprite = upSprite;
        }
        else if (movement.y < 0)
        {
            sr.sprite = downSprite;
            audioHandler.PlayStartWalkSound();
        }
        else{
            sr.sprite = idle;
        }
        
        
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            currentJumps = 0;
            audioHandler.PlayLandingSound();
        }
        if ((collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("Enemy")) && !isInvincible)
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            TakeDamage(1, contactPoint);
        }
        if(collision.gameObject.CompareTag("Void")){
            sr.color = Color.black;
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            TakeDamage(3, pushDirection);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(int damage, Vector2 contactPoint)
    {
        if (isInvincible || isDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        Vector2 pushDirection = (transform.position - (Vector3)contactPoint).normalized;

        float horizontalForce = pushBackForceHorizontal * pushDirection.x;
        
        // Always apply upward force for arc
        float verticalForce = pushBackForceVerticle;

        // Apply the combined force
        rb.velocity = Vector2.zero; // Reset current velocity
        rb.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
        audioHandler.PlayDamageSound();

        isDamage = true;  // Set knockback flag
        StartCoroutine(EndDamage());

        // Start invincibility
        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die(){
        audioHandler.PlayDeathSound();
        if(currentLives >1){
            currentLives --;
            RespawnAtCheckpoint();
            lm.livesCount--;
        }
        else{
            GameOver();
        }
    }
    private void GameOver(){
        Debug.Log("Game Over no more lives");
        SceneManager.LoadScene("GameOverScene");
    }

    private void RespawnAtCheckpoint()
    {
        if(CheckpointTrigger.IsCheckpointHit){
            currentHealth=maxHealth;
            transform.position = CheckpointTrigger.GetCheckpointPosition();
            UpdateHealthUI();
        }
        else{
            currentHealth=maxHealth;
            transform.position = initialSpawnPosiiton;
            UpdateHealthUI();
        }
    }
      private System.Collections.IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        // Optional: Add blinking effect
        StartCoroutine(BlinkEffect());
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    private IEnumerator EndDamage()
{
    yield return new WaitForSeconds(0.5f);  // Adjust time as needed
    isDamage = false;  // Re-enable movement after knockback period
}
    private System.Collections.IEnumerator BlinkEffect()
    {
        while (isInvincible)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void UpdateHealthUI()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].enabled = i < currentHealth;
        }
    }

    public void LoadSceneDelay(string sceneName){
        StartCoroutine(LoadSceneAfterDelay(sceneName));
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("coin")){
            Destroy(other.gameObject);
            audioHandler.PlayCoinCollectSound();
            cm.coinCount++;
        }
        switch (other.gameObject.tag){
            case "Portal":
            audioHandler.PlayPortalHitSound();
            Debug.Log("Loading Level1");
            LoadSceneDelay("Level1");
            break;
            case "Portal2":
            audioHandler.PlayPortalHitSound();
            Debug.Log("Loading LastLevel");
            LoadSceneDelay("LastLevel");
            break;
            case "BackPortal1":
            audioHandler.PlayPortalHitSound();
            Debug.Log("Loading Tutorial");
            LoadSceneDelay("Tutorial");
            break;
            case "BackPortal2":
            audioHandler.PlayPortalHitSound();
            Debug.Log("Loading Level1");
            LoadSceneDelay("Level1");
            break;
            case "Portal3":
            audioHandler.PlayPortalHitSound();
            LoadSceneDelay("GameOverScene");
            break;
            default:
            Debug.LogWarning("Unrecognized portal tag " + other.gameObject.tag);
            break;
        }
    }

   
       

}