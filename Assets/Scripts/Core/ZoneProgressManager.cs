// Assets/Scripts/Core/ZoneProgressManager.cs
using UnityEngine;
using UnityEngine.UI;

public class ZoneProgressManager : MonoBehaviour
{
    public static ZoneProgressManager Instance;

    [SerializeField] private Slider progressSlider;
    [SerializeField] private float holdTime = 3f;

    private float timer;
    private bool isInZone;
    private System.Action onComplete;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = holdTime;
            progressSlider.value = 0f;
            progressSlider.gameObject.SetActive(true); // всегда виден
        }
    }

    public void EnterZone(float customHoldTime, System.Action onCompleteAction)
    {
        holdTime = customHoldTime;
        progressSlider.maxValue = holdTime;
        isInZone = true;
        timer = 0f;
        onComplete = onCompleteAction;
    }

    public void ExitZone()
    {
        isInZone = false;
        timer = 0f;
        progressSlider.value = 0f;
    }

    private void Update()
    {
        if (!isInZone) return;

        timer += Time.deltaTime;
        progressSlider.value = timer;

        if (timer >= holdTime)
        {
            isInZone = false;
            progressSlider.value = 0f;
            onComplete?.Invoke();
        }
    }
}