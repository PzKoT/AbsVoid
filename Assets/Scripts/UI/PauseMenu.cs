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

    [Header("Settings")]
    [SerializeField] private string hubSceneName = "Hub";

    private bool isPaused = false;
    public static bool IsPaused { get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        // Создаём и включаем InputActions только для UI
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPausePerformed;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        resumeButton?.onClick.AddListener(Resume);
        exitToHubButton?.onClick.AddListener(ExitToHub);

        Debug.Log("✅ PauseMenu инициализирован");
    }

    private void OnDestroy()
    {
        // Перед уничтожением — убедимся, что управление включено
        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        if (inputActions != null)
        {
            inputActions.UI.Pause.performed -= OnPausePerformed;

            // Безопасно выключаем все карты
            if (inputActions.Player.enabled)
                inputActions.Player.Disable();
            if (inputActions.UI.enabled)
                inputActions.UI.Disable();

            inputActions.Dispose();
            inputActions = null;
        }

        Debug.Log("❎ PauseMenu уничтожен");
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

        Debug.Log("⏸ Игра на паузе");
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        IsPaused = false;

        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        Debug.Log("▶ Игра возобновлена");
    }

    public void ExitToHub()
    {
        // Восстанавливаем время и ввод ДО загрузки
        Time.timeScale = 1f;
        IsPaused = false;
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (GameInput.Instance != null)
            GameInput.Instance.EnablePlayerInput();

        Debug.Log("🏠 Переход в хаб...");
        SceneManager.LoadScene(hubSceneName);
    }
}
