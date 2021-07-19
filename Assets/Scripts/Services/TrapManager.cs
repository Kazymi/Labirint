using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private List<TrapConfiguration> trapConfigurations;

    private Dictionary<TrapType, Factory> _factories = new Dictionary<TrapType, Factory>();
    private void Start()
    {
        foreach (var trap in trapConfigurations)
        {
            CreateNewTrap(trap);
        }
    }

    private void OnEnable()
    {
        ServiceLocator.Subscribe<TrapManager>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unsubscribe<TrapManager>();
    }

    private void CreateNewTrap(TrapConfiguration trapConfiguration)
    {
        if (_factories.ContainsKey(trapConfiguration.TrapType))
        {
            _factories[trapConfiguration.TrapType].Prepolulate(trapConfiguration.TrapGameObject,trapConfiguration.StartCountInFactory);
        }
        else
        {
            _factories.Add(trapConfiguration.TrapType,new Factory(trapConfiguration.TrapGameObject,trapConfiguration.StartCountInFactory,transform));
        }
    }

    public GameObject GetTrapByType(TrapType trapType)
    {
        return _factories[trapType].Create();
    }

    public GameObject GetRandomTrapByType(TrapType trapType)
    {
        _factories[trapType].Mix();
        return _factories[trapType].Create();
    }
}
