using UnityEngine;
using UnityEngine.UI; // Required to access UI components

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Settings")]
    public float maxMana = 100f; // Maximum Mana
    public float currentMana;    // Current Mana value

    [Header("UI Settings")]
    public Image ManaBarImage;   // Reference to the UI Image for the Mana bar

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the player's current Mana to maxMana
        currentMana = maxMana;

        // Set the Mana bar to full at the start
        UpdateManaBar();
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes: Reduce Mana when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // Simulate taking 10 damage
        }
    }

    // Method to reduce the player's Mana
    public void TakeDamage(float damageAmount)
    {
        currentMana -= damageAmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Ensure Mana doesn't go below 0

        // Update the Mana bar UI after taking damage
        UpdateManaBar();

        // Check if player Mana reaches zero
        if (currentMana <= 0)
        {
            NoMana();
        }
    }

    // Method to update the Mana bar UI
    void UpdateManaBar()
    {
        // Calculate the fill amount (between 0 and 1) based on current Mana
        float fillAmount = currentMana / maxMana;

        // Update the Mana bar image's fill amount
        if (ManaBarImage != null)
        {
            ManaBarImage.fillAmount = fillAmount;
        }
    }

    // Method called when the player's Mana reaches zero
    void NoMana()
    {
        Debug.Log("Player No Mana");
        // Add death-related logic here (e.g., respawning, game over, etc.)
    }
}
