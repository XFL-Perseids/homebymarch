



namespace MyGame.Player
{
using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public int health;
    public int attack;
    public int defense;
    public int attackSpeed;
    public int experience;
    public int mana;
    public int attackRange;
    public Inventory inventory;
    public Transform transform;

    public PlayerData()
    {
        // Default values for a new player
        playerName = "New Player";
        level = 1;
        health = 100;
        attack = 10;
        defense = 5;
        mana = 20;
        experience = 0;
        attackRange = 50;
        inventory = new Inventory();
    }

    // Method to level up the player
    public void LevelUp()
    {
        level++;
        health += 10;
        attack += 5;
        defense += 3;
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        int damageTaken = damage - defense;
        health -= (damageTaken > 0) ? damageTaken : 0;
        if (health < 0) health = 0;
    }

    // Method to heal the player
    public void Heal(int amount)
    {
        health += amount;
        // Cap health at a maximum value, e.g., 100 + (10 * level)
        int maxHealth = 100 + (10 * level);
        if (health > maxHealth) health = maxHealth;
    }

    // Method to gain experience
    public void GainExperience(int exp)
    {
        experience += exp;
        // Example: Level up every 100 experience points
        if (experience >= 100 * level)
        {
            experience -= 100 * level;
            LevelUp();
        }
    }

    internal void Initialize()
    {
        throw new NotImplementedException();
    }
}
}