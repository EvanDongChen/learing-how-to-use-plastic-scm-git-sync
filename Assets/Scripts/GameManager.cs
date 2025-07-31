using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState state;
    public static event Action<GameState> OnStateChange;

    //public properties
    public bool waveClear {  get; set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //prevent gameobject from being destroyed during new scene load
            DontDestroyOnLoad(gameObject);
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
        throw new NotImplementedException();
    }

    private void runMainGame()
    {
        throw new NotImplementedException();
    }

    private void runStartMenu()
    {
        throw new NotImplementedException();
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