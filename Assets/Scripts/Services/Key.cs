using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView), typeof(Collider), typeof(Rigidbody))]
public class Key : MonoBehaviourPunCallbacks
{

    private KeyManager _keyManager;
    private PhotonView _photonView;
    private PlayerStatistics _playerStatistics;
    private bool _keyUnlock;

    public bool KeyUnlock
    {
        set => _keyUnlock = value;
    }

    private void Start()
    {
        _playerStatistics = ServiceLocator.GetService<PlayerStatistics>();
        _photonView = GetComponent<PhotonView>();
        _keyManager = ServiceLocator.GetService<KeyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatistics>())
        {
            Die();
        }
    }

    [PunRPC]
    public void SetPosition(Vector3 pos, Quaternion rot)
    {
        _keyUnlock = true;
        transform.position = pos;
        transform.rotation = rot;
    }

    private void Die()
    {
        if (_keyUnlock == false)
        {
            return;
        }
        _keyUnlock = false;
        _keyManager.PhotonViewMain.RPC(RPCEventType.DestroyGameObject, RpcTarget.All, _photonView.ViewID);
        _playerStatistics.AddFoundKey();
    }
}