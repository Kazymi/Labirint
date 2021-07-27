using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private bool mobile;

    private event Action _interactionAction;
    private event Action _punchAction;
    private event Action _pausedAction;
    
    public event Action InteractionAction
    {
        add => _interactionAction += value;
        remove => _interactionAction -= value;
    }
    
    public event Action PunchAction
    {
        add => _punchAction += value;
        remove => _punchAction -= value;
    }

    public event Action PausedAction
    {
        add => _pausedAction += value;
        remove => _pausedAction -= value;
    }
    
    private void OnEnable()
    {
        ServiceLocator.Subscribe<InputHandler>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<InputHandler>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
          _interactionAction?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _punchAction?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pausedAction?.Invoke();
        }
    }

    public Vector2 MoveDirection()
    {
        if (mobile)
        {
            return -_playerJoystick.Direction;
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}