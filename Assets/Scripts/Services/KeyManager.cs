using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PhotonView))]
public class KeyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject keysGameObjects;
    [SerializeField] private int countKey;
    
    private ChunkGenerator _chunkGenerator;
    private Dictionary<int, Key> keys = new Dictionary<int, Key>();
    private PhotonView _photonView;
    private FactoryPhoton _factoryPhoton;

    public PhotonView PhotonViewMain => _photonView;
    public override void OnEnable()
    {
        ServiceLocator.Subscribe<KeyManager>(this);
    }

    public override void OnDisable()
    {
        ServiceLocator.Unsubscribe<KeyManager>();
    }
    
    [PunRPC]
    public void DestroyGameObject(int idPhotonView)
    {
        if(PhotonNetwork.IsMasterClient == false) return;
        SetPositionKey(keys[idPhotonView],100);
    }

    public void StartKeyManager()
    {
        _photonView = GetComponent<PhotonView>();
        Random.InitState(ServiceLocator.GetService<SeedGenerator>().Seed);
    }

    public void Initialize(ChunkGenerator chunkGenerator)
    {
        _chunkGenerator = chunkGenerator;
        if(PhotonNetwork.IsMasterClient == false) return;
        _factoryPhoton = new FactoryPhoton(keysGameObjects.name, 0, transform);
        for (int i = 0; i != countKey; i++)
        {
            var newKey = _factoryPhoton.Create();
            var photonId = newKey.GetComponent<PhotonView>().ViewID;
            var key = newKey.GetComponent<Key>();
            SetPositionKey(key,100);
            keys.Add(photonId,key);
        }
    }
    
    private void SetPositionKey(Key key,int countRecursion)
    {
        countRecursion--;
        var chunk = _chunkGenerator.SpawnedChunk[Random.Range(0,_chunkGenerator.SpawnedChunk.Count)];
        var position = chunk.PositionKeys[Random.Range(0, chunk.PositionKeys.Count)];
        if (position.transform.childCount != 0)
        {
            if(countRecursion <= 0) return;
            SetPositionKey(key,countRecursion);
            key.KeyUnlock = true;
            return;
        }
        var photonKey = key.GetComponent<PhotonView>();
        photonKey.RPC("SetPosition",RpcTarget.All,position.position,position.rotation);
    }
}