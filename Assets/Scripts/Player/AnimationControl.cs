using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    private int _animationSpeedHash = Animator.StringToHash("Speed");
    private int _animationDieHash = Animator.StringToHash("Die");
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
            _animator.SetFloat(_animationSpeedHash, _inputHandler.MoveDirection().magnitude);
        }
    }

    public void Initialized(InputHandler inputHandler,Animator animator)
    {
        _animator = animator;
        _inputHandler = inputHandler;
    }

    public void Die()
    {
        _alive = false;
        _animator.SetBool(_animationDieHash,true);
    }
}