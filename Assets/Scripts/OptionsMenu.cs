using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button backButton;

    [Header("Key Rebinders")]
    [SerializeField] private KeyRebinder forwardRebinder;
    [SerializeField] private KeyRebinder backwardRebinder;
    [SerializeField] private KeyRebinder leftRebinder;
    [SerializeField] private KeyRebinder rightRebinder;
    [SerializeField] private KeyRebinder dashRebinder;
    [SerializeField] private KeyRebinder attackRebinder;

    private PlayerInputActions inputActions;

    private void Awake()
    {

        inputActions = GameManager.Instance.InputActions;

        inputActions.Enable();


        string savedBindings = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(savedBindings))
        {
            inputActions.LoadBindingOverridesFromJson(savedBindings);
        }

        forwardRebinder.Initialize(inputActions.Player.Move, 1);   // W / Up
        backwardRebinder.Initialize(inputActions.Player.Move, 2);  // S / Down
        leftRebinder.Initialize(inputActions.Player.Move, 3);      // A / Left
        rightRebinder.Initialize(inputActions.Player.Move, 4);     // D / Right
        dashRebinder.Initialize(inputActions.Player.Dash, 0);
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        backButton.onClick.AddListener(OnBackPressed);

        RegisterRebindSave(forwardRebinder);
        RegisterRebindSave(backwardRebinder);
        RegisterRebindSave(leftRebinder);
        RegisterRebindSave(rightRebinder);
        RegisterRebindSave(dashRebinder);
        RegisterRebindSave(attackRebinder);
    }

    private void RegisterRebindSave(KeyRebinder rebinder)
    {
        rebinder.OnRebindComplete += () =>
        {
            string bindings = inputActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", bindings);
            PlayerPrefs.Save();
            inputActions.LoadBindingOverridesFromJson(bindings);
            if (InputManager.Instance != null && InputManager.Instance.InputActions != inputActions)
            {
                InputManager.Instance.InputActions.LoadBindingOverridesFromJson(bindings);
            }
        };
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        Debug.Log($"Volume changed to: {value}");
    }

    private void OnBackPressed()
    {
        GameManager.Instance.UnPause();
        string previousScene = SceneTracker.PreviousScene;

        SceneManager.LoadScene(previousScene);

        if (previousScene == "Game" && GameManager.Instance != null)
        {
            GameManager.Instance.updateGameState(GameState.MainGame);
        }
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        backButton.onClick.RemoveListener(OnBackPressed);
    }
}
