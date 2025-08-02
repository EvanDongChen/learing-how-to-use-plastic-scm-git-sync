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
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        // Initialize KeyRebinders with the correct actions and binding indices
        forwardRebinder.Initialize(inputActions.Player.Move, 1);   // W / Up
        backwardRebinder.Initialize(inputActions.Player.Move, 2);  // S / Down
        leftRebinder.Initialize(inputActions.Player.Move, 3);      // A / Left
        rightRebinder.Initialize(inputActions.Player.Move, 4);     // D / Right

        dashRebinder.Initialize(inputActions.Player.Dash, 0);
        //attackRebinder.Initialize(inputActions.Player.Attack, 0); // Uncomment if using
    }

    private void Start()
    {
        // Volume setup
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        backButton.onClick.AddListener(OnBackPressed);
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        Debug.Log($"Volume changed to: {value}");
    }

    private void OnBackPressed()
    {
        GameManager.Instance.ReturnToPreviousContext();
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        backButton.onClick.RemoveListener(OnBackPressed);
    }
}
