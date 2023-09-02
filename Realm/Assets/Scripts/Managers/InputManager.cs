using System;
using UnityEngine;

public interface IInputManager
{
    event Action<Vector2> PlayerMovedFixed;
    event Action PlayerNotMovedFixed;
    event Action<Vector2> PlayerMoved;
    event Action PlayerNotMoved;
    event Action<Vector2> PointerMoved;
    event Action<Vector2> PointerClicked;
    event Action<float> OnScroll;
    event Action PlayerPressedSpaceBar;
}

public class InputManager : MonoBehaviour, IInputManager
{
    // Events

    public event Action<Vector2> PlayerMovedFixed;
    public event Action PlayerNotMovedFixed;

    public event Action<Vector2> PlayerMoved;
    public event Action PlayerNotMoved;
    
    public event Action<Vector2> PointerMoved;
    public event Action<Vector2> PointerClicked;

    public event Action<float> OnScroll;
    public event Action PlayerPressedSpaceBar;
    
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

        _inputActions.Player.SpaceBar.performed += (_) => PlayerPressedSpaceBar?.Invoke();
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
        
        var scrollDelta = _inputActions.Player.Scroll.ReadValue<float>();
        if(scrollDelta != 0)
            OnScroll?.Invoke(scrollDelta);
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
