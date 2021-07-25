using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //TODO: weird canvas structure
public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menus;

    public void OpenMenu(Menu menuToOpen)
    {
        // TODO: can remove Menu class and just use Canvas. 
        /*foreach (var menu in menus)
        {
            menu.Close();
        }*/
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Open)
            {
                CloseMenu(menus[i]);
            }
        }
        menuToOpen.OpenCanvas();
    }

    private void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}