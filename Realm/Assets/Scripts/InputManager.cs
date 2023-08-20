using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    // Events

    public event Action<Vector2> PlayerMovedFixed;
    public event Action PlayerNotMovedFixed;

    public event Action<Vector2> PlayerMoved;
    public event Action PlayerNotMoved;
    
    public event Action<Vector2> PointerMoved;
    public event Action<Vector2> PointerClicked;

    // Dependencies
    
    private InputActions _inputActions;
    
    // Cache

    private Vector2 _cachedPointerPosition;


    // Start is called before the first frame update
    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();

        
        _inputActions.Player.PointerPosition.performed += context =>
        {
            var pointerPosition = context.ReadValue<Vector2>();
            _cachedPointerPosition = pointerPosition;
            PointerMoved?.Invoke(pointerPosition);
        };
        _inputActions.Player.Fire.performed += context =>
        {
            PointerClicked?.Invoke(_cachedPointerPosition);
        };
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }
    
    private void OnDisable()
    {
        _inputActions.Player.Enable();
    }

    private void Update()
    {
        var move = _inputActions.Player.Move.ReadValue<Vector2>();
        if (move.magnitude > 0)
            PlayerMoved?.Invoke(move);
        else
            PlayerNotMoved?.Invoke();
    }

    private void FixedUpdate()
    {
        var move = _inputActions.Player.Move.ReadValue<Vector2>();
        if (move.magnitude > 0)
            PlayerMovedFixed?.Invoke(move);
        else
            PlayerNotMovedFixed?.Invoke();
    }
}
