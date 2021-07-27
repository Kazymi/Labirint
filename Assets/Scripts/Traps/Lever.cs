using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator), typeof(PhotonView))]
public class Lever : MonoBehaviourPunCallbacks, ITrapSetting
{
    [SerializeField] private float coolDown = 10f;
    [SerializeField] private float timer = 7f;

    private List<LeaverActivator> _listActivators;
    private Animator _animator;
    private PhotonView _photonView;
    private bool _activated;
    private int _idCurrentActivator;

    private const string _animationDownName = "Down";
    private const string _animationUpName = "Up";

    public PhotonView PhotonView
    {
        get => _photonView;
        set => _photonView = value;
    }

    public List<LeaverActivator> ListActivators
    {
        set => _listActivators = value;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    public void StartAction()
    {
        _photonView.RPC("Enable", RpcTarget.All);
    }

    [PunRPC]
    public void Initialize(int idChunk)
    {
        _listActivators = PhotonNetwork.GetPhotonView(idChunk).gameObject.GetComponent<Chunk>().ListActivators;
    }

    [PunRPC]
    public void Enable()
    {
        if (_activated) return;
        _animator.Play(_animationDownName, 0, 0);
        _idCurrentActivator = Random.Range(0, _listActivators.Count);
        _listActivators[_idCurrentActivator].gameObject.SetActive(true);
        _listActivators[_idCurrentActivator].Activate();
        StartCoroutine(Deactivate());
    }

    [PunRPC]
    public void Disable()
    {
        _listActivators[_idCurrentActivator].Deactivate();
    }

    public void SetPosition(Vector3 vector3, Quaternion quaternionSerializable)
    {
    }

    private IEnumerator Deactivate()
    {
        _activated = true;
        yield return new WaitForSeconds(timer);
        _photonView.RPC("Disable", RpcTarget.All);
        _animator.Play(_animationUpName, 0, 0);
        yield return new WaitForSeconds(coolDown - timer);
        _listActivators[_idCurrentActivator].gameObject.SetActive(false);
        _activated = false;
    }
}