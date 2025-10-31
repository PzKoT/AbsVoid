using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private Button skipButton;
    [SerializeField] private float displayTime = 3f;
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup panelCanvasGroup;
    private bool isShowingMessage = false;

    private void Awake()
    {
        Debug.Log("TutorialController: Awake вызван!");

        panelCanvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
            panelCanvasGroup = tutorialPanel.AddComponent<CanvasGroup>();

        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipTutorial);
            skipButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SkipButton не назначена в инспекторе!");
        }
    }

    private void Start()
    {
        Debug.Log("IsFirstLaunch: " + TutorialManager.IsFirstLaunch());

        if (TutorialManager.IsFirstLaunch())
        {
            StartCoroutine(ShowTutorialSequence());
        }
    }

    private IEnumerator ShowTutorialSequence()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ShowMessage("Используй WASD или стрелки, чтобы двигаться", displayTime));
        yield return StartCoroutine(ShowMessage("Нажми Esc для открытия меню", displayTime));
        yield return StartCoroutine(ShowMessage("Уничтожай врагов, получай опыт, прокачивай умения. Удачной игры!", displayTime));

        TutorialManager.CompleteTutorial();
    }

    private IEnumerator ShowMessage(string message, float time)
    {
        if (isShowingMessage) yield break;
        isShowingMessage = true;

        Debug.Log("Показываем: " + message);
        tutorialText.text = message;

        // Кнопка — мгновенно
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(true);
        }

        // Панель — с анимацией
        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;

        yield return StartCoroutine(FadeInPanel());

        yield return new WaitForSeconds(time);

        yield return StartCoroutine(FadeOutPanel());

        // Скрываем кнопку
        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }

        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        isShowingMessage = false;
    }

    private void SkipTutorial()
    {
        StopAllCoroutines();
        StartCoroutine(ForceHidePanel());
        TutorialManager.CompleteTutorial();
    }

    private IEnumerator ForceHidePanel()
    {
        isShowingMessage = false;

        yield return StartCoroutine(FadeOutPanel());

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
        }

        panelCanvasGroup.alpha = 0;
        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeInPanel()
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            yield return null;
        }
        panelCanvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutPanel()
    {
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            yield return null;
        }
        panelCanvasGroup.alpha = 0;
    }

    // ВАЖНО: Публичный метод для вызова из PauseMenu
    public void StartTutorialManually()
    {
        Debug.Log("Запуск туториала вручную...");

        // Сбрасываем прогресс
        PlayerPrefs.DeleteKey("TutorialCompleted");
        PlayerPrefs.Save();

        // Останавливаем текущий туториал (если был)
        StopAllCoroutines();

        // Запускаем заново
        StartCoroutine(ShowTutorialSequence());
    }
}