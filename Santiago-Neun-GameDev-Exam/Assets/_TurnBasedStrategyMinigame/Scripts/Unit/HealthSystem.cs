using System;
using UniRx;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Subject<Unit> OnDeath = new Subject<Unit>();
    public Subject<Unit> OnDamaged = new Subject<Unit>();

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

        OnDamaged.OnNext(Unit.Default);

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        OnDeath.OnNext(Unit.Default);
    }

    public float GetHealthNormalized()
    {
        return (float)health / _maxHealth;
    }

}
