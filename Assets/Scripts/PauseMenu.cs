using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;

    private PlayerInputActions inputActions;
    private GameManager gameManager;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Pause.performed += ctx =>
        {
            Debug.Log("Pause action performed");
            TogglePauseMenu();
        };
        GameManager.OnStateChange += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManagerOnStateChanged;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        pauseMenuPanel.SetActive(false);

        if (GameManager.Instance != null && GameManager.Instance.state == GameState.PausePage)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    private void GameManagerOnStateChanged(GameState state)
    {
        //toggle the pause menu ui based on the pausepage state
        if (state == GameState.PausePage)
        {
            pauseMenuPanel.SetActive(true);
        }
        else
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuPanel.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        GameManager.Instance.updateGameState(GameState.PausePage);
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.ResumeGameFromPause();
    }

    public void Options()
    {
        Debug.Log("Clicked Options");
        GameManager.Instance.updateGameState(GameState.OptionsMenu);
    }

    public void MainMenu()
    {
        Debug.Log("Clicked Menu");

        gameManager.updateGameState(GameState.StartMenu);
    }
}
