using UnityEngine;

public class CameraBackground : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform background;
    [SerializeField] private float referenceCameraSize = 10;

    [SerializeField] private float pullToCenter = 1.05f;

    private void OnEnable()
    {
        cameraMovement.Moved += OnCameraMoved;
        cameraMovement.Zoomed += OnCameraZoomed;
    }

    private void OnCameraZoomed()
    {
        var relativeCameraSize = camera.orthographicSize / referenceCameraSize;

        background.localScale = new Vector3(relativeCameraSize, relativeCameraSize);
    }

    private void OnCameraMoved()
    {
        var cameraPosition = camera.transform.position;
        background.localPosition = new Vector3(-cameraPosition.x / pullToCenter, -cameraPosition.y / pullToCenter);
    }
}