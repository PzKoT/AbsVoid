using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpValue = 10;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.3f;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // Боббинг (покачивание)
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public int Pickup()
    {
        Debug.Log($"+{xpValue} XP!");
        Destroy(gameObject);
        return xpValue;
    }
}