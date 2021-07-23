using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(CharacterController),typeof(PhotonView))]
public class PlayerPunch : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform punchTranform;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private float kickStrength = 3f;
    private PhotonView photonViewMain;
    
    private bool _punched;
    private CharacterController character;
    private Vector3 impact = Vector3.zero;
    
    private void Awake()
    {
        character = GetComponent<CharacterController>();
        photonViewMain = GetComponent<PhotonView>();
    }
    
    private void Update()
    {
        impact = new Vector3(impact.x, 0, impact.z);
        if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
    }

    public void Punch()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(punchTranform.position, punchTranform.forward, out raycastHit,maxDistance))
        {
            var constructor = raycastHit.transform.GetComponent<PlayerConstructor>();
            if (constructor)
            {
                var i = constructor.PlayerPunch;
                i.photonViewMain.RPC("GetPunch",RpcTarget.All,raycastHit.point,kickStrength);
            }
        }
    }

    [PunRPC]
    public void GetPunch(Vector3 point,float kickStrength)
    {
        var dir = (transform.position - point).normalized;
        if (dir.y < 0) dir.y = -dir.y;
        impact += dir.normalized * kickStrength;
    }
}