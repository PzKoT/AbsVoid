using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Объект, за которым следует камера (задаётся в Inspector или ищется автоматически)
    [SerializeField] private float smoothSpeed = 1f; // Скорость сглаживания
    [SerializeField] private Vector3 offset; // Смещение камеры от персонажа

    [System.Obsolete]
    private void Awake()
    {
        // Автоматический поиск объекта с компонентом Player, если target не задан вручную
        if (target == null)
        {
            Player person = FindObjectOfType<Player>();
            target = person.transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return; // Проверка, задан ли target, чтобы избежать ошибок

        // Целевая позиция камеры
        Vector3 desiredPosition = target.position + offset;

        // Сглаживание движения
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Применяем позицию (оставляем Z-координату камеры неизменной для 2D)
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}