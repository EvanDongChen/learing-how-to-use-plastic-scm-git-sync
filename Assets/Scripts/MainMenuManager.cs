using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.updateGameState(GameState.MainGame);
    }

    public void Options()
    {
        GameManager.Instance.updateGameState(GameState.OptionsMenu);
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
