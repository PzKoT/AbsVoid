using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRate = 1f; // ‡Á ‚ ÒÂÍÛÌ‰Û

    private float nextAttackTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Õ≈ Õ¿ÕŒ—»“‹ ”–ŒÕ ƒ–”√»Ã —À¿…Ã¿Ã!
        if (other.CompareTag("Enemy")) return;

        if (Time.time >= nextAttackTime)
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                nextAttackTime = Time.time + attackRate;

                // ŒÚÚ‡ÎÍË‚‡ÌËÂ
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 pushDir = (other.transform.position - transform.position).normalized;
                    rb.AddForce(pushDir * 4f, ForceMode2D.Impulse);
                }
            }
        }
    }

}
