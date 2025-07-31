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
    }

    private void OnEnable()
    {
        inputActions.Enable();

        if (gameManager == null)
        {
            gameManager = GameManager.Instance;

            if (gameManager == null)
            {
                Debug.LogError("GameManager instance not found. Make sure GameManager exists in the scene.");
            }
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
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
        if (gameManager != null)
        {
            gameManager.Pause();
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        if (gameManager != null)
        {
            gameManager.UnPause();
            gameManager.updateGameState(GameState.MainGame);
        }
    }

    public void Options()
    {
        if (gameManager != null)
        {
            gameManager.updateGameState(GameState.OptionsMenu);
        }
        
        SceneTracker.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    public void MainMenu()
    {
        if (gameManager != null)
        {
            gameManager.updateGameState(GameState.StartMenu);
        }
        SceneManager.LoadScene("Start");
    }
}
