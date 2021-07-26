using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    // TODO: private int test = Animator.StringToHash("Speed");
    private const string _animationSpeedName = "Speed";
    private const string _animationDieName = "Die";
    private InputHandler _inputHandler;
    private bool _alive = true;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_alive)
        {
            // TODO: use something like _inputHandler.InputMagnitude - Property declared in InputHandler
            _animator.SetFloat(_animationSpeedName, _inputHandler.MoveDirection().magnitude);
        }
    }

    public void Initialized(InputHandler inputHandler, Animator animator)
    {
        _animator = animator;
        _inputHandler = inputHandler;
    }

    public void Die()
    {
        _alive = false;
        _animator.SetBool(_animationDieName, true);
    }
}