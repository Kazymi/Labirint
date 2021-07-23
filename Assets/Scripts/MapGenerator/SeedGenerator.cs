using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeedGenerator : MonoBehaviourPunCallbacks
{
    private int _seed;

    public int Seed => _seed;

    public override void OnEnable()
    {
        base.OnEnable();
        ServiceLocator.Subscribe<SeedGenerator>(this);
    }

    public override void OnDisable()
    {
        ServiceLocator.Unsubscribe<SeedGenerator>();
    }
    
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int seed = Random.Range(100000, 999999);
            photonView.RPC("SeedGenerated", RpcTarget.All, seed);
        }
        else
        {
            photonView.RPC("GetSeed", RpcTarget.MasterClient,photonView.Controller);
        }
    }

    [PunRPC]
    public void GetSeed(Player player)
    {
        photonView.RPC("SeedGenerated",player,_seed);
    }

    [PunRPC]
    public void SeedGenerated(int seed)
    {
        _seed = seed;
    }
}