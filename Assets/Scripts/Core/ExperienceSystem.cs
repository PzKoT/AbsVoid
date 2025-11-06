using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class LevelUpEventArgs : EventArgs
{
    public int newLevel;
    public int xpGained;
}

public class ExperienceSystem : MonoBehaviour
{
    [Header("XP Settings")]
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private AnimationCurve xpCurve = AnimationCurve.EaseInOut(1, 0, 10, 1); // Нелинейный рост

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Slider xpBar;
    [SerializeField] private TMPro.TMP_Text levelText;

    // События
    public static event Action<LevelUpEventArgs> OnLevelUp;
    public static event Action<int> OnXPChanged;

    private int xpForNextLevel;

    private void Awake()
    {
        CalculateXPForNextLevel();
        UpdateUI();
    }

    public void AddXP(int xp)
    {
        currentXP += xp;
        OnXPChanged?.Invoke(currentXP);

        while (currentXP >= xpForNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= xpForNextLevel;
        currentLevel++;

        CalculateXPForNextLevel();

        LevelUpEventArgs args = new LevelUpEventArgs
        {
            newLevel = currentLevel,
            xpGained = xpForNextLevel
        };

        OnLevelUp?.Invoke(args);
        Debug.Log($"УРОВЕНЬ ВВЕРХ! {currentLevel}. Осталось XP: {currentXP}/{xpForNextLevel}");
    }

    private void CalculateXPForNextLevel()
    {
        xpForNextLevel = Mathf.RoundToInt(Mathf.Lerp(100, 1000, xpCurve.Evaluate(currentLevel - 1)));
    }

    private void UpdateUI()
    {
        if (xpBar != null)
            xpBar.value = (float)currentXP / xpForNextLevel;

        if (levelText != null)
            levelText.text = $"Ур. {currentLevel}";
    }

    // Геттеры для других скриптов
    public int CurrentLevel => currentLevel;
    public int XPForNextLevel => xpForNextLevel;
    public int CurrentXP => currentXP;
}