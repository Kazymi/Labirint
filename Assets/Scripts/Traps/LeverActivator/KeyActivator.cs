using Photon.Pun;
using UnityEngine;

public class KeyActivator : LeaverActivator
{
    [SerializeField] private Transform positionKey;

    private KeyManager _keyManager;
    
    public override void Activate()
    {
        if (_keyManager == null)
        {
            _keyManager = ServiceLocator.GetService<KeyManager>();
        }
        _keyManager.PhotonViewMain.RPC(RPCEventType.MoveRandomKey,RpcTarget.MasterClient,positionKey.position);
    }
}
