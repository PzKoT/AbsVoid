// Assets/Scripts/UI/UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject characterSelectPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenSettings() => settingsPanel.SetActive(true);
    public void OpenCharacterSelect() => characterSelectPanel.SetActive(true);
}