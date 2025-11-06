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
        [SerializeField] private float attackInterval = 3f;

        private float nextAttackTime = 0f;

        private void Update()
        {
            // АВТОМАТИЧЕСКАЯ АТАКА
            if (Time.time >= nextAttackTime)
            {
                Use(); // ← ВЫЗЫВАЕМ Use()
                nextAttackTime = Time.time + attackInterval;
            }
        }

        // ← ОБЯЗАТЕЛЬНО ПЕРЕОПРЕДЕЛЯЕМ!
        public override void Use()
        {
            Transform target = GetNearestEnemy();
            if (target == null) return;

            for (int i = 0; i < projectileCount; i++)
            {
                Vector2 dir = (target.position - player.transform.position).normalized;
                GameObject proj = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
                Projectile p = proj.GetComponent<Projectile>();
                p.Initialize(dir, projectileSpeed, damage, player.gameObject);
            }

            Debug.Log("Авто-выстрел!");
        }

        // ПРОКАЧКА
        public void UpgradeCooldown(float reduction) => attackInterval = Mathf.Max(0.5f, attackInterval - reduction);
        public void UpgradeProjectiles(int count) => projectileCount += count;
        public void UpgradeRange(float range) => maxRange += range;
    }
}