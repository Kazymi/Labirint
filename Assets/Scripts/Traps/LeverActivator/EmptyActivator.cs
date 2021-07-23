using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EmptyActivator : LeaverActivator
{
    [SerializeField] private Transform positionKey;

    private KeyManager _keyManager;

    private void Start()
    {
        _keyManager = ServiceLocator.GetService<KeyManager>();
    }

    public override void Activate()
    {
        var photonKey = _keyManager.GetRandomKey().GetComponent<PhotonView>();
        photonKey.RPC("SetPosition",RpcTarget.All,positionKey.position,positionKey.rotation);
    }

    public override void Deactivate()
    {
        
    }
}
