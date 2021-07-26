using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Canvas[] menus;

    public void OpenMenu(Canvas menuToOpen)
    {
        foreach (var menu in menus)
        {
            menu.enabled = false;
        }
        menuToOpen.enabled = true;
    }
}