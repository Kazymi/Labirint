using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    [SerializeField] private Canvas loadMenu;
    [SerializeField] private Canvas gameMenu;

    private void Awake()
    {
        Preloading.Initialize(this);
    }

    public void PreloadingCompleted()
    {
        loadMenu.enabled = false;
        gameMenu.enabled = true;
    }
}
