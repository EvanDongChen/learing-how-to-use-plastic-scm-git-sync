using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private MusicSheetManager musicSheetManager;

    public Sprite quarterNoteIcon;

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
    public bool waveClear { get; set; }    public bool isEditable { get; private set; }
    public int currentWave { get; private set; } = 0;
    public bool isWaveActive { get; private set; } = false;
    public GameState previousState;

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
        InputActions = new PlayerInputActions();
        string savedBindings = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(savedBindings))
        {
            InputActions.asset.LoadBindingOverridesFromJson(savedBindings);
        }

        InputActions.Enable();
    }

    private void Start()
    {
        musicSheetManager = FindAnyObjectByType<MusicSheetManager>();
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
            case GameState.RoundStart:
                runRoundStart();
                break;
            case GameState.GamePlay:
                runGamePlay();
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
        Debug.Log("GameManager: Player has lost the game.");
        SceneManager.LoadScene("Start");
    }

    private void runRoundEnd()
    {
        Debug.Log("GameManager: Round ended. shop openeing");

        GameObject shopCanvas = GameObject.Find("ShopCanvas");
        if (shopCanvas != null)
        {
            shopCanvas.SetActive(true);
        }

        isEditable = true;
    }

    private void runRoundStart()
    {
        Debug.Log("GameManager: Round is starting. Player can edit music sheet.");
        isEditable = true;
    }

    private void runGamePlay()
    {
        Debug.Log("GameManager: Starting music playback.");
        isEditable = false;

        if (musicSheetManager == null)
        {
            musicSheetManager = FindAnyObjectByType<MusicSheetManager>();
        }

        if (musicSheetManager != null)
        {
            musicSheetManager.TogglePlayback();
        }
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
        GiveStarterNotes();
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

    public void StartWave()
    {
        currentWave++;
        isWaveActive = true;

        EnemySpawner enemySpawner = FindAnyObjectByType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.StartWave(currentWave);
        }
        else
        {
            Debug.LogWarning("EnemySpawner not found in the scene.");
        }
    }

    public void OnWaveEnd()
    {
        isWaveActive = false;
        waveClear = true;

        updateGameState(GameState.RoundEnd);
    }

    public void SetWaveActive(bool isActive)
    {
        isWaveActive = isActive;
    }

    private void GiveStarterNotes()
    {
        Debug.Log("Giving player starter notes...");

        NoteData fireNote = ScriptableObject.CreateInstance<NoteData>();
        fireNote.noteName = "Fire Quarter Note"; 
        fireNote.level = 1;
        fireNote.element = NoteData.Elements.Fire;
        fireNote.rarity = 1;
        fireNote.noteDuration = 1f;
        fireNote.icon = quarterNoteIcon;
        fireNote.attributes = new List<NoteData.AttributeType>();
        InventoryManager.Instance.AddNote(fireNote);

        // Create Water Note
        NoteData waterNote = ScriptableObject.CreateInstance<NoteData>();
        waterNote.noteName = "Water Quarter Note";
        waterNote.level = 1;
        waterNote.element = NoteData.Elements.Water;
        waterNote.rarity = 1;
        waterNote.noteDuration = 1f;
        waterNote.icon = quarterNoteIcon; // <-- Using the new, single icon
        waterNote.attributes = new List<NoteData.AttributeType>();
        InventoryManager.Instance.AddNote(waterNote);

        // Create Lightning Note
        NoteData lightningNote = ScriptableObject.CreateInstance<NoteData>();
        lightningNote.noteName = "Lightning Quarter Note";
        lightningNote.level = 1;
        lightningNote.element = NoteData.Elements.Lightining;
        lightningNote.rarity = 1;
        lightningNote.noteDuration = 1f;
        lightningNote.icon = quarterNoteIcon;
        lightningNote.attributes = new List<NoteData.AttributeType>();
        InventoryManager.Instance.AddNote(lightningNote);
    }


    // FOR TESTING PURPOSES
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("DEBUG: Changing to Round End State");
            updateGameState(GameState.RoundEnd);
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("DEBUG: Changing to Round Start State");
            updateGameState(GameState.RoundStart);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("DEBUG: Changing to GamePlay State");
            updateGameState(GameState.GamePlay);
        }
    }


}

public enum GameState
{
    StartMenu,
    MainGame,
    OptionsMenu,
    PausePage,
    RoundStart,
    GamePlay,
    RoundEnd,
    Lose,
    Win
}