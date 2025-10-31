using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitToHubButton;
    [SerializeField] private Button showTutorialButton;
    [SerializeField] private Button quitGameButton;

    [Header("Quit Confirmation")]
    [SerializeField] private GameObject quitConfirmPanel;     // Панель подтверждения
    [SerializeField] private Button confirmYesButton;         // "Да"
    [SerializeField] private Button confirmNoButton;          // "Нет"

    [Header("Settings")]
    [SerializeField] private string hubSceneName = "Hub";

    private TutorialController tutorialController;
    private bool isPaused = false;
    public static bool IsPaused { get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPausePerformed;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (quitConfirmPanel != null)
            quitConfirmPanel.SetActive(false);

        resumeButton?.onClick.AddListener(Resume);
        exitToHubButton?.onClick.AddListener(ExitToHub);
        showTutorialButton?.onClick.AddListener(ForceShowTutorial);
        quitGameButton?.onClick.AddListener(ShowQuitConfirmation);

        confirmYesButton?.onClick.AddListener(QuitGame);
        confirmNoButton?.onClick.AddListener(HideQuitConfirmation);

        tutorialController = Object.FindFirstObjectByType<TutorialController>();
        if (tutorialController == null)
        {
            Debug.LogWarning("TutorialController не найден в сцене!");
        }

        Debug.Log("PauseMenu инициализирован");
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        if (inputActions != null)
        {
            inputActions.UI.Pause.performed -= OnPausePerformed;
            if (inputActions.Player.enabled)
                inputActions.Player.Disable();
            if (inputActions.UI.enabled)
                inputActions.UI.Disable();
            inputActions.Dispose();
            inputActions = null;
        }

        Debug.Log("PauseMenu уничтожен");
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        IsPaused = true;

        if (GameInput.Instance != null)
            GameInput.Instance.DisablePlayerInput();

        Debug.Log("Игра на паузе");
    }

    public void Resume()
    {
        HideQuitConfirmation(); // На всякий случай

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;

        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        Debug.Log("Игра возобновлена");
    }

    public void ExitToHub()
    {
        HideQuitConfirmation();

        Time.timeScale = 1f;
        IsPaused = false;
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        Debug.Log("Переход в хаб...");
        SceneManager.LoadScene(hubSceneName);
    }

    private void ForceShowTutorial()
    {
        HideQuitConfirmation();

        if (tutorialController != null)
        {
            tutorialController.StartTutorialManually();
        }
        else
        {
            Debug.LogError("TutorialController не найден! Нельзя запустить туториал.");
        }
    }

    // Показать панель подтверждения
    private void ShowQuitConfirmation()
    {
        if (quitConfirmPanel != null)
        {
            quitConfirmPanel.SetActive(true);
        }
    }

    // Скрыть панель подтверждения
    private void HideQuitConfirmation()
    {
        if (quitConfirmPanel != null)
        {
            quitConfirmPanel.SetActive(false);
        }
    }

    // Выйти из игры
    private void QuitGame()
    {
        Debug.Log("Выход из игры...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}