using UnityEngine;

namespace Game.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] protected float cooldown = 3f;
        [SerializeField] protected int damage = 5;
        [SerializeField] protected float projectileSpeed = 10f;
        [SerializeField] protected float maxRange = 15f;

        protected float lastUseTime = -Mathf.Infinity;
        protected Player player;

        protected virtual void Awake()
        {
            player = GetComponentInParent<Player>();
        }

        public bool CanUse() => Time.time >= lastUseTime + cooldown;

        public abstract void Use();

        protected Transform GetNearestEnemy()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(
                player.transform.position, maxRange, LayerMask.GetMask("Enemy"));

            Transform nearest = null;
            float closest = maxRange;

            foreach (var e in enemies)
            {
                float dist = Vector2.Distance(player.transform.position, e.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    nearest = e.transform;
                }
            }
            return nearest;
        }
    }
}