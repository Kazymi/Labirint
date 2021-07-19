using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private float triggerRadius = 5;

    public void OnClick()
    {
        var allElement = Physics.OverlapSphere(transform.position, triggerRadius);
        foreach (var findElement in allElement.Where(t => t.GetComponent<TrapSetting>() != null))
        {
            findElement.GetComponent<TrapSetting>().StartAction();
        }
    }
}
