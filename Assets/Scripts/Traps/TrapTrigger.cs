using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Collider),typeof(PhotonView), typeof(Animator))]
public class TrapTrigger : MonoBehaviourPunCallbacks,TrapSetting
{
    [SerializeField] private float timerCooldown;
    [SerializeField] private float timerAction;

    private Animator _animator;
    private bool _activated;
    private const string animationNameOpen = "Open";
    private const string animationNameClose = "Close";
    
    public PhotonView PhotonView1 { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        PhotonView1 = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_activated) return;
        var i = other.GetComponent<PlayerHealth>();
        if (i == false) return;
        StartAction(i);
    }

    public void StartAction()
    {
        if(_activated) return;
        photonView.RPC("Disable", RpcTarget.All);
    }
    
    private void StartAction(PlayerHealth playerHealth)
    {
        playerHealth.Death();
        photonView.RPC("Disable", RpcTarget.All);
    }

    [PunRPC]
    public void Enable()
    {
        _activated = false;
    }

    [PunRPC]
    public void Disable()
    {
        _animator.Play(animationNameOpen,0,0);
        StartCoroutine(Activate());
    }

    [PunRPC]
    public void SetPosition(Vector3 vector3, Quaternion quaternionSerializable)
    {
        transform.position = vector3;
        transform.rotation = quaternionSerializable;
    }

    private IEnumerator Activate()
    {
        _activated = true;
        yield return new WaitForSeconds(timerAction);
        _animator.Play(animationNameClose,0,0);
        yield return new WaitForSeconds(timerCooldown - timerAction);
        photonView.RPC("Enable", RpcTarget.All);
    }
}