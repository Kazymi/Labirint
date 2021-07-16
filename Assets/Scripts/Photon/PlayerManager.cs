using UnityEngine;
using Photon.Pun;
using System.IO;
using Zenject;

public class PlayerManager : MonoBehaviour
{
    private PhotonView _pv;
    private GameObject _controller;
    private Transform _spawnpoint;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {
        _controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), _spawnpoint.position,
        _spawnpoint.rotation, 0, new object[] {_pv.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(_controller);
        CreateController();
    }

    [Inject]
    private void Constructor(SpawnManager spawnManager)
    {
        Debug.Log("SpawnPoint initialize");
        _spawnpoint = spawnManager.GetSpawnPoint();
    }
}