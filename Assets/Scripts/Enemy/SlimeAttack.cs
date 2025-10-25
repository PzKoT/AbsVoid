// Assets/Scripts/Enemy/SlimeAttack.cs
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && timer <= 0f)
        {
            col.gameObject.GetComponent<Health>()?.TakeDamage(damage);
            timer = attackCooldown;
        }
    }
}