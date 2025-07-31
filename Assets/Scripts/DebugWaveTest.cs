using UnityEngine;
using UnityEngine.InputSystem;  // <-- Important

public class DebugWaveTest : MonoBehaviour
{
    public TonearmController controller;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            controller.NextWave();
        }
    }
}
