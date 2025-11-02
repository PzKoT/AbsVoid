using UnityEngine;
using TMPro;

public class name : MonoBehaviour
{
    [SerializeField] private Transform target; // BossSlime
    [SerializeField] private Vector3 offset = new Vector3(0, 1.8f, 0);
    [SerializeField] private Camera cam;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target != null && cam != null)
        {
            transform.position = target.position + offset;
            transform.rotation = cam.transform.rotation; // всегда лицом к камере
        }
    }
}
