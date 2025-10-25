using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float drag = 5f;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float flipDelay = 0.3f;
    [SerializeField] private float rayAngle = 30f;
    [SerializeField] private float avoidanceSmoothing = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer visual;
    private Animator anim;
    private float directionTimer;
    private float lastDirectionX;
    private Vector2 currentDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        visual = transform.Find("SlimeVisual")?.GetComponent<SpriteRenderer>();
        anim = transform.Find("SlimeVisual")?.GetComponent<Animator>();
        if (visual == null)
        {
            Debug.LogError($"SlimeVisual не найден для слайма {gameObject.name}!");
        }
        if (anim == null)
        {
            Debug.LogError($"Animator не найден на SlimeVisual для слайма {gameObject.name}!");
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
        if (player == null || visual == null || anim == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newVelocity = AvoidObstacles(direction);
            currentDirection = Vector2.Lerp(currentDirection, newVelocity, avoidanceSmoothing * Time.fixedDeltaTime);
            rb.linearVelocity = currentDirection * moveSpeed;
            UpdateSpriteFlip(currentDirection.x);
            anim.SetBool("IsMoving", true);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * drag);
            directionTimer = 0f;
            currentDirection = Vector2.zero;
            anim.SetBool("IsMoving", false);
        }
    }

    private Vector2 AvoidObstacles(Vector2 targetDirection)
    {
        Vector2 rightRayDir = Quaternion.Euler(0, 0, -rayAngle) * targetDirection;
        Vector2 leftRayDir = Quaternion.Euler(0, 0, rayAngle) * targetDirection;

        RaycastHit2D hitRightRay = Physics2D.Raycast(transform.position, rightRayDir, rayDistance, obstacleLayer);
        RaycastHit2D hitLeftRay = Physics2D.Raycast(transform.position, leftRayDir, rayDistance, obstacleLayer);

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
                return -targetDirection.normalized;
            }
        }
        return targetDirection;
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
            anim.SetBool("IsFacingRight", directionX >= 0);
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