using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private InputManager _inputManager;
    private Transform _transform;

    private void Awake()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _transform = transform;
    }

    private void OnEnable()
    {
        _inputManager.PlayerMoved += InputManagerOnPlayerMoved;
    }

    private void InputManagerOnPlayerMoved(Vector2 move)
    {
        var position = _transform.position;

        var step = move * (speed * Time.deltaTime);
        
        _transform.position = position + (Vector3)step;
    }
    
}
