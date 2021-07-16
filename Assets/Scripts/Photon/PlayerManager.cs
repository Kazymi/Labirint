using UnityEngine;
using Photon.Pun;
using System.IO;

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
            _spawnpoint = ServiceLocator.GetService<SpawnManager>().GetSpawnPoint();
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
}