// using Math;
using UnityEngine;
using MyGame.Player;

namespace MyGame.Enemies
{


public abstract class Enemy
{
    public string enemyName;
    public int health;
    public int attackPower;
    public int defense;
    public float attackSpeed;
    public float attackRange;
    public Transform transform;
    private bool isAttackInCooldown = false;
    public PlayerData player;

    public Enemy(string name, int hp, int atk, int def, float atkSpd, float range)
    {
        enemyName = name;
        health = hp;
        attackPower = atk;
        defense = def;
        attackSpeed = atkSpd;
        attackRange = range;


        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Attack(){


        // if (Vector3.distance(this.position, player.position)){
        // // attack logic here
        // int damage = Math.Round((int)Mathf.Pow(this.attackPower, 2) / player.defense);
        // player.TakeDamage(damage);
        // CooldownAttack();
        // }
        
        

    }

    private void CooldownAttack(){
        

    }

    private void Die()
    {
        // Logic for when the enemy dies
    }


}
}