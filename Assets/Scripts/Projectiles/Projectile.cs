using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 2f;

        private Vector2 direction;
        private float speed;
        private int damage;
        private GameObject owner;

        public void Initialize(Vector2 dir, float spd, int dmg, GameObject own)
        {
            direction = dir.normalized;
            speed = spd;
            damage = dmg;
            owner = own;
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == owner) return;

            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}