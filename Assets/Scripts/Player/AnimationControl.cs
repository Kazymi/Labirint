using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    private const string _animationSpeedName = "Speed";
    private Animator _animator;
    private InputHandler _inputHandler;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat(_animationSpeedName,_inputHandler.MoveDirection().magnitude);
    }

    public void Initialized(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }
}
