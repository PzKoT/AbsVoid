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

        public override void Use()
        {
            if (!CanUse()) return;

            Transform target = GetNearestEnemy();
            if (target == null) return;

            for (int i = 0; i < projectileCount; i++)
            {
                // ТОЧНОЕ направление без разброса
                Vector2 direction = (target.position - transform.position).normalized;

                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Projectile p = proj.GetComponent<Projectile>();
                p.Initialize(direction, projectileSpeed, damage, player.gameObject);
            }

            lastUseTime = Time.time;
        }

        private void Update()
        {
            if (CanUse())
            {
                Transform target = GetNearestEnemy();
                if (target != null)
                {
                    Use();
                }
            }
        }

        public void UpgradeCooldown(float reduction) => cooldown = Mathf.Max(0.5f, cooldown - reduction);
        public void UpgradeProjectiles(int count) => projectileCount += count;
        public void UpgradeRange(float range) => maxRange += range;
    }
}