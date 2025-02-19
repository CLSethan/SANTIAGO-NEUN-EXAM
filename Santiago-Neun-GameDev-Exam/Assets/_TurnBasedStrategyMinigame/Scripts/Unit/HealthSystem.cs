using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;

    [SerializeField]
    private int health;
    [SerializeField]
    private int _maxHealth = 100;

    private void Awake()
    {
        _maxHealth = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / _maxHealth;
    }

}
