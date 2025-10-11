using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Скорость движения слайма
    [SerializeField] private float detectionRange = 5f; // Радиус обнаружения игрока
    [SerializeField] private Transform player; // Ссылка на игрока
    [SerializeField] private float mass = 1f; // Масса слайма
    [SerializeField] private float drag = 5f; // Сопротивление движения
    [SerializeField] private float rayDistance = 2f; // Длина луча для проверки препятствий
    [SerializeField] private LayerMask obstacleLayer; // Слой для препятствий
    [SerializeField] private float flipDelay = 0.1f; // Задержка перед флипом спрайта
    [SerializeField] private float rayAngle = 30f; // Угол для боковых лучей (градусы)
    [SerializeField] private float avoidanceSmoothing = 5f; // Сглаживание направления

    private Rigidbody2D rb;
    private SpriteRenderer visual;
    private float directionTimer; // Таймер для отслеживания направления
    private float lastDirectionX; // Последнее направление по X
    private Vector2 currentDirection; // Текущее направление движения

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        visual = transform.Find("SlimeVisual")?.GetComponent<SpriteRenderer>();
        if (visual == null)
        {
            Debug.LogError($"SlimeVisual не найден для слайма {gameObject.name}!");
        }
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError($"Игрок с тегом 'Player' не найден для слайма {gameObject.name}!");
                enabled = false;
            }
        }
        rb.mass = mass;
        rb.linearDamping = drag;
        currentDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (player == null || visual == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newVelocity = AvoidObstacles(direction);
            currentDirection = Vector2.Lerp(currentDirection, newVelocity, avoidanceSmoothing * Time.fixedDeltaTime);
            rb.linearVelocity = currentDirection * moveSpeed;
            UpdateSpriteFlip(currentDirection.x);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * drag);
            directionTimer = 0f;
            currentDirection = Vector2.zero;
        }
    }

    private Vector2 AvoidObstacles(Vector2 targetDirection)
    {
        // Два луча спереди под углом
        Vector2 rightRayDir = Quaternion.Euler(0, 0, -rayAngle) * targetDirection;
        Vector2 leftRayDir = Quaternion.Euler(0, 0, rayAngle) * targetDirection;

        RaycastHit2D hitRightRay = Physics2D.Raycast(transform.position, rightRayDir, rayDistance, obstacleLayer);
        RaycastHit2D hitLeftRay = Physics2D.Raycast(transform.position, leftRayDir, rayDistance, obstacleLayer);

        // Если хотя бы один луч свободен
        if (hitRightRay.collider == null && hitLeftRay.collider != null)
        {
            return rightRayDir.normalized;
        }
        else if (hitLeftRay.collider == null && hitRightRay.collider != null)
        {
            return leftRayDir.normalized;
        }
        else if (hitRightRay.collider != null && hitLeftRay.collider != null)
        {
            // Оба пути заблокированы — пробуем боковые пути
            Vector2 rightSideDir = Vector2.Perpendicular(targetDirection);
            Vector2 leftSideDir = -rightSideDir;

            RaycastHit2D hitRightSide = Physics2D.Raycast(transform.position, rightSideDir.normalized, rayDistance, obstacleLayer);
            RaycastHit2D hitLeftSide = Physics2D.Raycast(transform.position, leftSideDir.normalized, rayDistance, obstacleLayer);

            if (hitRightSide.collider == null)
            {
                return rightSideDir.normalized;
            }
            else if (hitLeftSide.collider == null)
            {
                return leftSideDir.normalized;
            }
            else
            {
                // Оба боковых пути заблокированы — отступаем назад
                return -targetDirection.normalized;
            }
        }
        return targetDirection; // Путь свободен
    }

    private void UpdateSpriteFlip(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.1f) return;

        if ((directionX > 0 && lastDirectionX <= 0) || (directionX < 0 && lastDirectionX >= 0))
        {
            directionTimer = 0f;
            lastDirectionX = directionX;
        }

        directionTimer += Time.fixedDeltaTime;
        if (directionTimer >= flipDelay)
        {
            visual.flipX = directionX < 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Gizmos.color = Color.red;
            Vector2 rightRayDir = Quaternion.Euler(0, 0, -rayAngle) * direction;
            Gizmos.DrawRay(transform.position, rightRayDir * rayDistance);

            Gizmos.color = Color.green;
            Vector2 leftRayDir = Quaternion.Euler(0, 0, rayAngle) * direction;
            Gizmos.DrawRay(transform.position, leftRayDir * rayDistance);

            Gizmos.color = Color.blue;
            Vector2 rightSideDir = Vector2.Perpendicular(direction);
            Gizmos.DrawRay(transform.position, rightSideDir.normalized * rayDistance);

            Gizmos.color = Color.yellow;
            Vector2 leftSideDir = -rightSideDir;
            Gizmos.DrawRay(transform.position, leftSideDir.normalized * rayDistance);
        }
    }
}