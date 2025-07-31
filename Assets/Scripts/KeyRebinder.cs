using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    public TMP_Text bindingDisplayText;
    public Button rebindButton;
    public int bindingIndex = 0; // For composite bindings, e.g. 0 for "up"

    private InputAction actionToRebind;

    // Call this to initialize the rebinder with the actual InputAction and binding index
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
        );
    }

    public void StartRebinding()
    {
        if (actionToRebind == null) return;

        rebindButton.interactable = false;
        bindingDisplayText.text = "Press any key...";

        // Disable action before rebinding
        actionToRebind.Disable();

        actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                operation.Dispose();

                // Save and re-enable
                SaveBindingOverride();
                UpdateBindingDisplay();
                actionToRebind.Enable();

                rebindButton.interactable = true;
                Debug.Log($"Rebound {actionToRebind.name} binding {bindingIndex} to {bindingDisplayText.text}");
            })
            .Start();
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
