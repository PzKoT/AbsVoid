using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] public int xpValue = 10;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float attractionSpeed = 5f;
    [SerializeField] private float pickupDistance = 0.3f;

    private Vector3 startPos;
    private bool isAttracted = false;
    private Transform player;
    private CircleCollider2D orbCollider; // переименовали переменную

    private void Awake()
    {
        startPos = transform.position;
        orbCollider = GetComponent<CircleCollider2D>();

        if (orbCollider != null)
        {
            orbCollider.isTrigger = true;
        }
    }

    private void Update()
    {
        if (isAttracted && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) <= pickupDistance)
            {
                Pickup();
            }
        }
        else
        {
            float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isAttracted = true;
            Debug.Log("ќрб прит€гиваетс€ к игроку");
        }
    }

    public int Pickup()
    {
        Debug.Log($"+{xpValue} XP!");
        Destroy(gameObject);
        return xpValue;
    }
}