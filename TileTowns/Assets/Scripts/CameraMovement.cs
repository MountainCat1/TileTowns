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
    
    [SerializeField] private Vector2 initialPositionOffset = new Vector2(1, -1);

    private IInputManager _inputManager;
    private IGameManager _gameManager;
    private Transform _transform;
    private Camera _camera;

    [Inject]
    public void Construct(IInputManager inputManager, IGameManager gameManager)
    {
        _inputManager = inputManager;
        _gameManager = gameManager;
        _transform = transform;
        _camera = GetComponentInChildren<Camera>();
        
        
        _gameManager.LevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded()
    {
        // Center camera when game starts
        // Get the bounds of the Tilemap
        BoundsInt bounds = _gameManager.Tilemap.cellBounds;

        // Calculate the center position
        Vector3 centerPosition = _gameManager.Tilemap.GetCellCenterWorld(new Vector3Int(bounds.x + bounds.size.x / 2, bounds.y + bounds.size.y / 2, 0)) + (Vector3)initialPositionOffset;

        transform.position = centerPosition;
        
        // Now you can use the 'centerPosition' for whatever you need
        Debug.Log("Center position of the Tilemap is " + centerPosition);
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
