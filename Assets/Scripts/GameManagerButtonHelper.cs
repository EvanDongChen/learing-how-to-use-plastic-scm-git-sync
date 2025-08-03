using UnityEngine;
using UnityEngine.UI;

public class GameManagerButtonHelper : MonoBehaviour
{
    public Button startRoundButton;
    public Button startWaveButton;

    private void OnEnable()
    {
        // Subscribe to the state change event
        GameManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnStateChange -= HandleGameStateChange;
    }

    private void Start()
    {
        // Add a listener to the StartRoundButton
        if (startRoundButton != null)
        {
            startRoundButton.onClick.AddListener(() => GameManager.Instance.BeginRound());
        }

        // Add a listener to the StartWaveButton
        if (startWaveButton != null)
        {
            startWaveButton.onClick.AddListener(() => GameManager.Instance.StartWave());
        }

        // IMPORTANT: Call the handler to set the initial button state
        if (GameManager.Instance != null)
        {
            HandleGameStateChange(GameManager.Instance.state);
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        // Hide all buttons by default
        if (startRoundButton != null) startRoundButton.gameObject.SetActive(false);
        if (startWaveButton != null) startWaveButton.gameObject.SetActive(false);

        switch (newState)
        {
            case GameState.MainGame:
                // Only the "Start Round" button should be active
                if (startRoundButton != null) startRoundButton.gameObject.SetActive(true);
                break;
            case GameState.RoundStart:
                // Only the "Start Wave" button should be active
                if (startWaveButton != null) startWaveButton.gameObject.SetActive(true);
                break;
            case GameState.GamePlay:
                // Both buttons should be hidden during gameplay
                break;
            case GameState.RoundEnd:
                // Only the "Start Round" button should be active again
                if (startRoundButton != null) startRoundButton.gameObject.SetActive(true);
                break;
            default:
                // Hide all buttons for other states
                break;
        }
    }
}