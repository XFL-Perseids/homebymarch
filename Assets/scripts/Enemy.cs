namespace MyGame.Enemies
{


public class Enemy
{
    public string enemyName;
    public int health;
    public int attackPower;

    public Enemy(string name, int hp, int atk)
    {
        enemyName = name;
        health = hp;
        attackPower = atk;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Logic for when the enemy dies
    }
}
}