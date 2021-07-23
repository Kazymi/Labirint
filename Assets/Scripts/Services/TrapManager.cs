using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private List<TrapConfiguration> trapConfigurations;

    private Dictionary<TrapType, FactoryPhoton> _factories = new Dictionary<TrapType, FactoryPhoton>();
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient == false) return;
        
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

    public GameObject GetTrapByType(TrapType trapType)
    {
        return _factories[trapType].Create();
    }

    public GameObject GetRandomTrapByType(TrapType trapType)
    {
        _factories[trapType].Mix();
        return _factories[trapType].Create();
    }
    
    private void CreateNewTrap(TrapConfiguration trapConfiguration)
    {
        if (_factories.ContainsKey(trapConfiguration.TrapType))
        {
            _factories[trapConfiguration.TrapType].Prepolulate(trapConfiguration.TrapGameObject.name,trapConfiguration.StartCountInFactory);
        }
        else
        {
            _factories.Add(trapConfiguration.TrapType,new FactoryPhoton(trapConfiguration.TrapGameObject.name,trapConfiguration.StartCountInFactory,transform));
        }
    }
}
