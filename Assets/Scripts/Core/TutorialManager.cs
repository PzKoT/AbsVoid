using UnityEngine;

public static class TutorialManager
{
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";

    public static bool IsFirstLaunch()
    {
        return PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0) == 0;
    }

    public static void CompleteTutorial()
    {
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
        PlayerPrefs.Save();
    }
}