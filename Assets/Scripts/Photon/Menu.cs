using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour
{
	[SerializeField] private bool open;
	private Canvas _canvas;

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();
	}

	public bool Open => open;

	public void OpenCanvas()
	{
		open = true;
		_canvas.enabled=true;
	}

	public void Close()
	{
		open = false;
		_canvas.enabled=false;
	}
}