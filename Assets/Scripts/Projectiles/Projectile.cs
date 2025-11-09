using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private LayerMask collisionLayers;

        private Vector2 direction;
        private float speed;
        private int damage;
        private GameObject owner;
        private Rigidbody2D rb;

        public void Initialize(Vector2 dir, float spd, int dmg, GameObject own)
        {
            direction = dir.normalized;
            speed = spd;
            damage = dmg;
            owner = own;

            rb = GetComponent<Rigidbody2D>();

            // Поворачиваем снаряд в направлении движения
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(gameObject, lifetime);
        }

        private void Start()
        {
            // Двигаем снаряд через Rigidbody для правильной физики
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Игнорируем коллизии с владельцем
            if (other.gameObject == owner) return;

            // Игнорируем коллизии с другими снарядами (по слою)
            if (other.gameObject.layer == gameObject.layer) return;

            // Проверяем слой коллизии
            if (((1 << other.gameObject.layer) & collisionLayers) != 0)
            {
                Health health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
    }
}