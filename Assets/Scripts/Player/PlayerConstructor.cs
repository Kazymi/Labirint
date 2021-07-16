using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConstructor : MonoBehaviour
{
    [SerializeField] private Movenment _movenment;
    [SerializeField] private InputHandler _inputHandler;

    private void Start()
    {
        _movenment.Initialize(_inputHandler);
    }
}
