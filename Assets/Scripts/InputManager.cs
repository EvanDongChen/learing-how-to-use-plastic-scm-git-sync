using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerInputActions InputActions { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InputActions = new PlayerInputActions();
        InputActions.Enable();

        LoadAllBindingOverrides();
    }

    private void LoadAllBindingOverrides()
    {
        foreach (var map in InputActions.asset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    string key = $"{action.name}_{i}_binding";
                    string savedBinding = PlayerPrefs.GetString(key, "");
                    if (!string.IsNullOrEmpty(savedBinding))
                    {
                        action.ApplyBindingOverride(i, savedBinding);
                    }
                }
            }
        }
    }

    public void SaveAllBindingOverrides()
    {
        foreach (var map in InputActions.asset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var overridePath = action.bindings[i].overridePath;
                    string key = $"{action.name}_{i}_binding";
                    if (!string.IsNullOrEmpty(overridePath))
                    {
                        PlayerPrefs.SetString(key, overridePath);
                    }
                    else
                    {
                        PlayerPrefs.DeleteKey(key);
                    }
                }
            }
        }
        PlayerPrefs.Save();
    }
}
