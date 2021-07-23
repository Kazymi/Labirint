using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Joystick _playerJoystick;
    [SerializeField] private bool mobile;
    
    private PlayerTrigger _playerTrigger;
    private PlayerPunch _playerPunch;
    
    private void Awake()
    {
        ServiceLocator.Subscribe<InputHandler>(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _playerTrigger.OnClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _playerPunch.Punch();
        }
    }
    
    public void Initialize(PlayerTrigger playerTrigger,PlayerPunch playerPunch)
    {
        _playerPunch = playerPunch;
        _playerTrigger = playerTrigger;
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
