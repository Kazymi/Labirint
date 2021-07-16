using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

	[SerializeField] Menu[] menus;


	public void OpenMenu(Menu menu)
	{
		for(int i = 0; i < menus.Length; i++)
		{
			if(menus[i].Open)
			{
				CloseMenu(menus[i]);
			}
		}
		menu.OpenCanvas();
	}

	public void CloseMenu(Menu menu)
	{
		menu.Close();
	}
}