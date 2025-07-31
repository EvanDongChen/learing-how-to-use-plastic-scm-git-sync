using UnityEngine;
using UnityEngine.InputSystem;

public class CameraViewSwitcher : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerTransform;  // Your player Transform
    public Transform mapViewAnchor;

    public Canvas musicSheet;   // Assign your music sheet GameObject here

    public float playerZoom = 6f;
    public float mapZoom = 20f;
    public float zoomSpeed = 5f;

    private float targetZoom;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;

        cameraFollow.target = playerTransform;
        targetZoom = playerZoom;

        musicSheet.gameObject.SetActive(true);  // Show music sheet initially since player view
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            cameraFollow.target = mapViewAnchor;
            targetZoom = mapZoom;

            musicSheet.gameObject.SetActive(false);  // Hide music sheet in map view
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            cameraFollow.target = playerTransform;
            targetZoom = playerZoom;

            musicSheet.gameObject.SetActive(true);   // Show music sheet in player view
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }
}
