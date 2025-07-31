using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTracker
{
    public static string PreviousScene { get; private set; } = "Start"; 
    public static void SetPreviousScene(string sceneName)
    {
        PreviousScene = sceneName;
    }
}
