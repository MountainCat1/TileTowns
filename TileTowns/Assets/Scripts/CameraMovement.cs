using System;
using UnityEngine;
using Zenject;


public class CameraMovement : MonoBehaviour
{
    public event Action Zoomed;
    public event Action Moved;
    
    [SerializeField] private float speed = 5f;
    [SerializeField] private float zoomSensitivity = 1f;
    [SerializeField] private float minZoom = 2;
    [SerializeField] private float maxZoom = 20;

    private IInputManager _inputManager;
    private Transform _transform;
    private Camera _camera;

    [Inject]
    public void Construct(IInputManager inputManager)
    {
        _inputManager = inputManager;
        _transform = transform;
        _camera = GetComponentInChildren<Camera>();
    }
    
    private void OnEnable()
    {
        _inputManager.PlayerMoved += InputManagerOnPlayerMoved;
        _inputManager.OnScroll += InputManagerOnOnScroll;
    }

    private void InputManagerOnOnScroll(float delta)
    {
        var orthographicSize = _camera.orthographicSize;
        
        orthographicSize -= delta * zoomSensitivity; // Notice the minus sign to make it zoom in when delta is positive
        _camera.orthographicSize = orthographicSize;
        _camera.orthographicSize = Mathf.Clamp(orthographicSize, minZoom, maxZoom); // Optional, to limit zoom
        
        Zoomed?.Invoke();
    }

    private void InputManagerOnPlayerMoved(Vector2 move)
    {
        var position = _transform.position;

        var step = move * (speed * Time.deltaTime);
        
        _transform.position = position + (Vector3)step;
        
        Moved?.Invoke();
    }
    
}
