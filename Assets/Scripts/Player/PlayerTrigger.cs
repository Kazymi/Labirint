using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private float triggerRadius = 2f;
    [SerializeField] private float cooldown=2f;

    private bool _unlockTrigger = true;
    private InputHandler _inputHandler;

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PunchAction += Punch;
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PunchAction -= Punch;
        }
    }

    private void Start()
    {
        _inputHandler = ServiceLocator.GetService<InputHandler>();
        _inputHandler.PunchAction += Punch;
    }

    private void Punch()
    {
        if(_unlockTrigger == false) return;
        var allElement = Physics.OverlapSphere(transform.position, triggerRadius);
        foreach (var findElement in allElement.Where(t => t.GetComponent<ITrapSetting>() != null))
        {
            findElement.GetComponent<ITrapSetting>().StartAction();
        }

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        _unlockTrigger = false;
        yield return new WaitForSeconds(cooldown);
        _unlockTrigger = true;
    }
    
}
