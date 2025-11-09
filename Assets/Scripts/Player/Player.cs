using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private float movingSpeed = 5f;

    [Header("Pickup")]
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private LayerMask xpOrbLayer = 1 << 9;

    [Header("Systems")]
    [SerializeField] private ExperienceSystem experienceSystem;

    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
        PickupXPOrbs();
    }

    private void PickupXPOrbs()
    {
        Collider2D[] orbs = Physics2D.OverlapCircleAll(transform.position, pickupRadius, xpOrbLayer);

        //Debug.Log($"Найдено орбов: {orbs.Length}");

        foreach (Collider2D orbCollider in orbs)
        {
            XPOrb orb = orbCollider.GetComponent<XPOrb>();
            if (orb != null)
            {
                int xp = orb.Pickup();
                experienceSystem.AddXP(xp);
                Debug.Log($"Подобран орб: +{xp} XP");
            }
        }
    }

    private void Movement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
            isRunning = true;
        else
            isRunning = false;
    }

    public bool IsRunning() => isRunning;

    public Vector3 GetPlayerScreenPosition() => Camera.main.WorldToScreenPoint(transform.position);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

    // Убираем устаревшие методы FindObjectsOfType
}