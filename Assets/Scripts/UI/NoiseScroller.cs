using UnityEngine;
using UnityEngine.UI;

public class NoiseScroller : MonoBehaviour
{
    [SerializeField] private RawImage noiseImage;
    [SerializeField] private float speed = 0.05f;

    private void Awake()
    {
        if (noiseImage == null)
        {
            noiseImage = GetComponent<RawImage>();
            if (noiseImage == null)
            {
                Debug.LogError("RawImage не найден! Прикрепи в инспекторе или добавь на объект.");
                enabled = false;
            }
        }
    }
    
    private void Update()
    {
        if (noiseImage == null) return;
        Rect currentRect = noiseImage.uvRect;
        currentRect.x = Mathf.Repeat(currentRect.x + speed * Time.deltaTime, 1f);
        currentRect.y = Mathf.Repeat(currentRect.y + speed * 0.5f * Time.deltaTime, 1f);
        noiseImage.uvRect = currentRect;
    }
}