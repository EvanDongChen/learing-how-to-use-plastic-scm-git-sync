using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerInputActions InputActions { get; private set; }
    public GameState state;
    public static event Action<GameState> OnStateChange;

    //public properties
    public bool waveClear {  get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InputActions = new PlayerInputActions();

            string savedBindings = PlayerPrefs.GetString("rebinds", string.Empty);
            if (!string.IsNullOrEmpty(savedBindings))
            {
                InputActions.asset.LoadBindingOverridesFromJson(savedBindings);
            }

            InputActions.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        updateGameState(GameState.StartMenu);
    }

    public void updateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.StartMenu:
                UnPause();
                runStartMenu();
                break;
            case GameState.MainGame:
                runMainGame();
                break;
            case GameState.OptionsMenu:
                runOptionsMenu();
                break;
            case GameState.RoundEnd:
                runRoundEnd();
                break;
            case GameState.Lose:
                runLose();
                break;
            case GameState.Win:
                runWin();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnStateChange?.Invoke(newState);
    }

    private void runWin()
    {
        throw new NotImplementedException();
    }

    private void runLose()
    {
        throw new NotImplementedException();
    }

    private void runRoundEnd()
    {
        throw new NotImplementedException();
    }

    private void runOptionsMenu()
    {
        Debug.Log("Options Menu logic runs here.");
        // TODO: Add actual Options Menu code
    }

    private void runMainGame()
    {
        Debug.Log("Main Game logic runs here.");
        // TODO: Add actual Main Game code
    }

    private void runStartMenu()
    {
        Debug.Log("Start Menu logic runs here.");
        // TODO: Add actual Start Menu code
    }


    ///////////Methods///////////
    public void Pause() => Time.timeScale = 0f;
    public void UnPause() => Time.timeScale = 1f;


}

public enum GameState
{
    StartMenu,
    MainGame,
    OptionsMenu,
    RoundEnd,
    Lose,
    Win
}