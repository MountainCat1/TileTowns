using UnityEngine;
using Zenject;


public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private IInputManager _inputManager;
    private Transform _transform;

    [Inject]
    public void Construct(IInputManager inputManager)
    {
        _inputManager = inputManager;
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
