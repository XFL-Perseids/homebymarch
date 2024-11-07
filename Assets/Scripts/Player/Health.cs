using UnityEngine;

namespace HomeByMarch
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel playerHealthChannel;

        public delegate void DamageTaken();
        public event DamageTaken OnDamageTaken;

        int currentHealth;

        public bool IsDead => currentHealth <= 0;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        void Start()
        {
            PublishHealthPercentage();
        }

        public void TakeDamage(int damage)
        {
            if (IsDead) return; // Prevent taking damage if already dead

            currentHealth -= damage;
            PublishHealthPercentage();
            OnDamageTaken?.Invoke(); // Notify listeners that damage was taken

            if (IsDead)
            {
                Debug.Log("Player is dead!");
            }
        }

        void PublishHealthPercentage()
        {
            if (playerHealthChannel != null)
                playerHealthChannel.Invoke(currentHealth / (float)maxHealth);
        }
    }
}
