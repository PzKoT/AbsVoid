using Game.Abilities;
using Game.Projectiles;
using UnityEngine;

namespace Game.Abilities
{
    public class MagicShot : Ability
    {
        [Header("Magic Shot")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int projectileCount = 1;

        // УБИРАЕМ attackInterval - используем cooldown из базового класса

        public override void Use()
        {
            if (!CanUse()) return;

            Transform target = GetNearestEnemy();
            if (target == null)
            {
                Debug.Log("Нет целей для атаки!");
                return;
            }

            for (int i = 0; i < projectileCount; i++)
            {
                // Правильное вычисление направления
                Vector2 direction = (target.position - transform.position).normalized;

                // Случайное смещение для разброса (опционально)
                Vector2 spread = Random.insideUnitCircle * 0.1f;
                Vector2 finalDirection = (direction + spread).normalized;

                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Projectile p = proj.GetComponent<Projectile>();
                p.Initialize(finalDirection, projectileSpeed, damage, player.gameObject);
            }

            lastUseTime = Time.time;
            Debug.Log($"Выстрел в направлении: {target.position}");
        }

        private void Update()
        {
            // ПРОСТОЙ И НАДЕЖНЫЙ АВТОВЫСТРЕЛ
            if (CanUse())
            {
                Transform target = GetNearestEnemy();
                if (target != null)
                {
                    Use();
                }
            }
        }

        // ПРОКАЧКА
        public void UpgradeCooldown(float reduction) => cooldown = Mathf.Max(0.5f, cooldown - reduction);
        public void UpgradeProjectiles(int count) => projectileCount += count;
        public void UpgradeRange(float range) => maxRange += range;
    }
}