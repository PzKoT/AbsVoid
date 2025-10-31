// Assets/Scripts/Enemy/SlimeAttack.cs
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private float lastAttackTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, есть ли Health у объекта
        Health health = other.GetComponent<Health>();
        if (health != null && Time.time >= lastAttackTime + attackCooldown)
        {
            health.TakeDamage(damage);
            lastAttackTime = Time.time;

            Debug.Log($"Слайм нанёс {damage} урона!");
        }
    }
}