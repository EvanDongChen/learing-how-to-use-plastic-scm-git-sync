using UnityEngine;
using UnityEngine.InputSystem;

public class TestCameraSwitch : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerViewAnchor;
    public Transform mapViewAnchor;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        inputActions.Player.SwitchToMap.performed += ctx => SwitchToMapView();
        inputActions.Player.SwitchToPlayer.performed += ctx => SwitchToPlayerView();
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    private void SwitchToMapView()
    {
        Debug.Log("Switched to Map View");
        cameraFollow.target = mapViewAnchor;
    }

    private void SwitchToPlayerView()
    {
        Debug.Log("Switched to Player View");
        cameraFollow.target = playerViewAnchor;
    }
}
