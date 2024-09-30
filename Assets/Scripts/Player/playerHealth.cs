using UnityEngine;
using UnityEngine.UI; // Required to access UI components
using TMPro;         // Required to use TextMeshProUGUI

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f; // Maximum health
    public float currentHealth;    // Current health value

    [Header("UI Settings")]
    public Image healthBarImage;         // Reference to the UI Image for the health bar
    public TextMeshProUGUI healthText;   // Reference to the TextMeshProUGUI for health display

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's current health to maxHealth
        currentHealth = maxHealth;

        // Set the health bar and text to full at the start
        UpdateHealthBar();
        UpdateHealthText();
    }

    // Method to reduce the player's health when called from another script
    public void OnButtonClickReduceHealth(float damageAmount)
    {
        TakeDamage(damageAmount); // Call the TakeDamage method to reduce health
    }

    // Method to reduce the player's health
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        // Update the health bar UI and health text after taking damage
        UpdateHealthBar();
        UpdateHealthText();

        // Check if player health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to update the health bar UI
    public void UpdateHealthBar()
    {
        // Calculate the fill amount (between 0 and 1) based on current health
        float fillAmount = currentHealth / maxHealth;

        // Update the health bar image's fill amount
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = fillAmount;
        }
    }

    // Method to update the health text UI
    public void UpdateHealthText()
    {
        // Display the current and max health as a string (e.g., "90/100")
        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }

    // Method called when the player's health reaches zero
    void Die()
    {
        Debug.Log("Player Died");
        // Add death-related logic here (e.g., respawning, game over, etc.)
    }
}
