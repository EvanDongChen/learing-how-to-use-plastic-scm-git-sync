using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    public PlayerInputActions InputActions { get; private set; }
    public GameState state;
    public static event Action<GameState> OnStateChange;

    //public properties
    public bool waveClear {  get; set; }
    public GameState previousState;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            //prevent gameobject from being destroyed during new scene load
            DontDestroyOnLoad(gameObject);
        }

        // Initialize the new Input System actions
        inputActions = new PlayerInputActions();
        string savedBindings = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(savedBindings))
        {
            InputActions.asset.LoadBindingOverridesFromJson(savedBindings);
        }

        inputActions.Enable();
    }

    private void Start()
    {
        updateGameState(GameState.StartMenu);
    }

    public void updateGameState(GameState newState)
    {
        previousState = state;
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
            case GameState.PausePage:
                runPausePage();
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

    //STATE FUNCTIONS //////////
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
        Debug.Log("Game Manager: Round ended | Shop opening");
    }

    private void runPausePage()
    {
        Pause();
    }

    private void runOptionsMenu()
    {
        Debug.Log("GameManager: Loading Options Page");
        SceneManager.LoadScene("Options");
    }

    private void runMainGame()
    {
        Debug.Log("GameManager: Loading Game");
        SceneManager.LoadScene("Game");
    }

    private void runStartMenu()
    {
        Debug.Log("GameManager: Loading Start Menu");
        SceneManager.LoadScene("Start");
    }


    ///////////Methods///////////
    public void Pause() => Time.timeScale = 0f;
    public void UnPause() => Time.timeScale = 1f;

    public void ResumeGameFromPause()
    {
        UnPause();
        updateGameState(GameState.MainGame);
    }



    //special case revert for pause
    public void ReturnToPreviousContext()
    {
        if (previousState == GameState.PausePage || previousState == GameState.MainGame)
        {
            SceneManager.LoadScene("Game");
        }
        else if (previousState == GameState.StartMenu)
        {
            SceneManager.LoadScene("Start");
        }
        updateGameState(previousState);
    }


    // FOR TESTING PURPOSES
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("DEBUG: Changing to Round End State");
            updateGameState(GameState.RoundEnd);
        }
    }


}

public enum GameState
{
    StartMenu,
    MainGame,
    OptionsMenu,
    PausePage,
    RoundEnd,
    Lose,
    Win
}