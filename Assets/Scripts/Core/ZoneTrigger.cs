// Assets/Scripts/Core/ZoneTrigger.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType { Game, Settings, CharacterSelect }

    [SerializeField] private ZoneType zoneType;
    [SerializeField] private float holdTime = 3f;
    [SerializeField] private string targetScene = "GameLevel";

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        ZoneProgressManager.Instance?.EnterZone(holdTime, ActivateZone);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        ZoneProgressManager.Instance?.ExitZone();
    }

    private void ActivateZone()
    {
        switch (zoneType)
        {
            case ZoneType.Game:
                SceneManager.LoadScene(targetScene);
                break;
            case ZoneType.Settings:
                UIManager.Instance?.OpenSettings();
                break;
            case ZoneType.CharacterSelect:
                UIManager.Instance?.OpenCharacterSelect();
                break;
        }
    }
}