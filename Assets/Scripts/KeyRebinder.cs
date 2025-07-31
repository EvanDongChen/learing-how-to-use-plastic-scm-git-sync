using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    public TMP_Text bindingDisplayText;
    public Button rebindButton;
    public int bindingIndex = 0;

    private InputAction actionToRebind;


    public event System.Action OnRebindComplete;

    public void Initialize(InputAction action, int bindingIdx = 0)
    {
        actionToRebind = action;
        bindingIndex = bindingIdx;

        UpdateBindingDisplay();
        rebindButton.onClick.AddListener(StartRebinding);
        LoadBindingOverride();
    }

    void UpdateBindingDisplay()
    {
        if (actionToRebind == null || bindingIndex >= actionToRebind.bindings.Count) return;

        var binding = actionToRebind.bindings[bindingIndex];
        bindingDisplayText.text = InputControlPath.ToHumanReadableString(
            binding.effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        ).ToUpper();
    }

    public void StartRebinding()
    {
        if (actionToRebind == null) return;

        rebindButton.interactable = false;
        bindingDisplayText.text = "Press any key...";

        actionToRebind.Disable();

        actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                string newBindingPath = operation.selectedControl.path;
                actionToRebind.ApplyBindingOverride(bindingIndex, newBindingPath);

                operation.Dispose();

                SaveBindingOverride();
                UpdateBindingDisplay();
                actionToRebind.Enable();

                SyncOverridesGlobally();
                rebindButton.interactable = true;
                Debug.Log($"Rebound {actionToRebind.name} binding {bindingIndex} to {bindingDisplayText.text}");

                SyncOverridesGlobally();

                OnRebindComplete?.Invoke();
            })
            .Start();
    }

    private void SyncOverridesGlobally()
    {
        if (InputManager.Instance == null) return;

        string json = InputManager.Instance.InputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", json);
        PlayerPrefs.Save();

        InputManager.Instance.InputActions.LoadBindingOverridesFromJson(json);

        InputManager.Instance.InputActions.Disable();
        InputManager.Instance.InputActions.Enable();

        var playerInput = InputManager.Instance.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions.Disable();
            playerInput.actions.Enable();
            Debug.Log("PlayerInput.actions disabled and enabled");
        }

        Debug.Log("Global InputActions updated and re-enabled with new bindings");
    }





    void SaveBindingOverride()
    {
        string key = $"{actionToRebind.name}_{bindingIndex}_binding";
        string path = actionToRebind.bindings[bindingIndex].overridePath;
        PlayerPrefs.SetString(key, path);
        PlayerPrefs.Save();
    }

    public void LoadBindingOverride()
    {
        string key = $"{actionToRebind.name}_{bindingIndex}_binding";
        string savedBinding = PlayerPrefs.GetString(key, "");

        if (!string.IsNullOrEmpty(savedBinding))
        {
            actionToRebind.ApplyBindingOverride(bindingIndex, savedBinding);
            UpdateBindingDisplay();
        }
    }
}
