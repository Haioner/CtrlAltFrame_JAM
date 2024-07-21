using UnityEngine.Events;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private bool canDamage;
    [SerializeField] private float MaxHealth;
    private float currentHealth;

    [Space]
    [SerializeField] private UnityEvent OnDamage;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void Damage(float damage)
    {
        if(currentHealth > 0)
        {
            if (canDamage)
                currentHealth -= damage;

            OnDamage?.Invoke();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}
