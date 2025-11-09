using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpValue = 10;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.3f;
    [SerializeField] private float attractionSpeed = 3f; // ƒќЅј¬№“≈ прит€жение

    private Vector3 startPos;
    private bool isAttracted = false;
    private Transform player;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isAttracted && player != null)
        {
            // ѕлавное прит€жение к игроку
            transform.position = Vector3.MoveTowards(transform.position, player.position, attractionSpeed * Time.deltaTime);
        }
        else
        {
            // ќбычное парение
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

            // јвтоподбор при касании
            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                Pickup();
            }
        }
    }

    public int Pickup()
    {
        Debug.Log($"+{xpValue} XP!");
        Destroy(gameObject);
        return xpValue;
    }
}