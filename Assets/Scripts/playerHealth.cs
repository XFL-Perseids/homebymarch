using UnityEngine;
using UnityEngine.UI; // Required to access UI components

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health
    public float currentHealth;    // Current health value

    [Header("UI Settings")]
    public Image healthBarImage;   // Reference to the UI Image for the health bar

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's current health to maxHealth
        currentHealth = maxHealth;

        // Set the health bar to full at the start
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes: Reduce health when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // Simulate taking 10 damage
        }
    }

    // Method to reduce the player's health
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        // Update the health bar UI after taking damage
        UpdateHealthBar();

        // Check if player health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update the health bar UI
    void UpdateHealthBar()
    {
        // Calculate the fill amount (between 0 and 1) based on current health
        float fillAmount = currentHealth / maxHealth;

        // Update the health bar image's fill amount
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = fillAmount;
        }
    }

    // Method called when the player's health reaches zero
    void Die()
    {
        Debug.Log("Player Died");
        // Add death-related logic here (e.g., respawning, game over, etc.)
    }
}
