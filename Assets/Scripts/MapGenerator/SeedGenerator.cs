using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeedGenerator : MonoBehaviourPunCallbacks
{
    private int _seed;

    public int Seed => _seed;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int seed = Random.Range(100000, 999999);
            photonView.RPC("SeedGenerated", RpcTarget.All, seed);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        ServiceLocator.Subscribe<SeedGenerator>(this);   
        
    }

    public override void OnDisable()
    {
        ServiceLocator.Unsubscribe<SeedGenerator>();
    }

    [PunRPC]
    public void SeedGenerated(int seed)
    {
        _seed = seed;
    }
}
