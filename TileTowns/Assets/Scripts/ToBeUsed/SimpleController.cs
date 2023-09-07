using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class SimpleController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private CharacterAnimator _characterAnimator;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _characterAnimator = GetComponent<CharacterAnimator>();
    }
    
    private void Start()
    {
    }

    private void Update()
    {
        // Get the movement input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        var movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

        var step = movementDirection * speed;

        if (step.magnitude > 0)
        {
            _characterAnimator.PlayAnimation(CharacterAnimator.Animation.Walk);
        }
        else
        {
            _characterAnimator.PlayAnimation(CharacterAnimator.Animation.Idle);
        }

        _transform.position = (Vector2)_transform.position + step;;
    }
}