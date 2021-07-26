using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView), typeof(Collider), typeof(Rigidbody))]
public class Key : MonoBehaviourPunCallbacks
{
    private KeyManager _keyManager;
    private PhotonView _photonView;
    private PlayerStatistics _playerStatistics;
    private bool keyUnlock = true;

    public bool KeyUnlock
    {
        set => keyUnlock = value;
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
        transform.position = pos;
        transform.rotation = rot;
    }

    private void Die()
    {
        if (keyUnlock == false) return;
        keyUnlock = false;
        _keyManager.PhotonViewMain.RPC(RPCEventType.DestroyGameObject, RpcTarget.All, _photonView.ViewID);
        _playerStatistics.AddFoundKey();
    }
}