using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick playerJoystick;
    [SerializeField] private bool mobile;

    [SerializeField] private Button interactionButton;
    [SerializeField] private Button punchButton;
    [SerializeField] private Button pausedButton;
    
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
        pausedButton?.onClick.AddListener(() => _pausedAction?.Invoke());
        punchButton?.onClick.AddListener(() => _punchAction?.Invoke());
        interactionButton?.onClick.AddListener(() => _interactionAction?.Invoke());
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<InputHandler>();
        pausedButton?.onClick.RemoveListener(() => _pausedAction?.Invoke());
        punchButton?.onClick.RemoveListener(() => _punchAction?.Invoke());
        interactionButton?.onClick.RemoveListener(() => _interactionAction?.Invoke());
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
            return -playerJoystick.Direction;
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}