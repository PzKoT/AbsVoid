using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Скорость движения слайма
    [SerializeField] private float detectionRange = 5f; // Радиус обнаружения игрока
    [SerializeField] private Transform player; // Ссылка на игрока
    [SerializeField] private float mass = 1f; // Масса слайма
    [SerializeField] private float drag = 5f; // Сопротивление движения
    [SerializeField] private float rayDistance = 1.5f; // Длина луча для проверки препятствий
    [SerializeField] private LayerMask obstacleLayer; // Слой для препятствий (деревья, кусты)

    private Rigidbody2D rb; // Компонент Rigidbody2D слайма

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Игрок с тегом 'Player' не найден!");
            }
        }
        rb.mass = mass;
        rb.linearDamping = drag;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newVelocity = AvoidObstacles(direction);
            rb.linearVelocity = newVelocity * moveSpeed;
            FlipSprite(newVelocity.x);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * drag);
        }
    }

    private Vector2 AvoidObstacles(Vector2 targetDirection)
    {
        // Проверяем путь с помощью луча
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, rayDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // Препятствие найдено — пробуем обойти вправо или влево
            Vector2 avoidDirection = Vector2.Perpendicular(targetDirection) * (Random.value > 0.5f ? 1f : -1f);
            return avoidDirection.normalized;
        }

        return targetDirection;
    }

    private void FlipSprite(float directionX)
    {
        SpriteRenderer visual = transform.Find("SlimeVisual")?.GetComponent<SpriteRenderer>();
        if (visual != null)
        {
            if (directionX > 0)
            {
                visual.flipX = false; // Смотрит вправо
            }
            else if (directionX < 0)
            {
                visual.flipX = true; // Смотрит влево
            }
        }
    }

    // Для отладки (опционально): рисуем лучи в редакторе
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Vector2 direction = (player.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction * rayDistance);
        }
    }
}