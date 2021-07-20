using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    private const string _animationSpeedName = "Speed";
    private const string _animationDieName = "Die";
    private Animator _animator;
    private InputHandler _inputHandler;
    private bool _alive = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_alive)
            _animator.SetFloat(_animationSpeedName, _inputHandler.MoveDirection().magnitude);
    }

    public void Initialized(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    public void Die()
    {
        _alive = false;
        _animator.Play(_animationDieName);
    }
}