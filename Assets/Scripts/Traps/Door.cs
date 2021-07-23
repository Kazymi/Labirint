using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView),typeof(Animator))]
public class Door : MonoBehaviourPunCallbacks,TrapSetting
{
   [SerializeField] private float cooldown = 12f;
   [SerializeField] private float openingTime = 5f;

   private const string _animationOpenName = "Open";
   private const string _animationCloseName = "Close";
   private bool _opened;
   private Animator _animator;
   public PhotonView PhotonView1 { get; set; }
   
   private void Awake()
   {
      _animator = GetComponent<Animator>();
   }

   private void Start()
   {
      PhotonView1 = GetComponent<PhotonView>();
   }

   public void StartAction()
   {
      if(_opened) return;
      PhotonView1.RPC("Disable", RpcTarget.All);
   }

   [PunRPC]
   public void Enable()
   {
      _opened = false;
   }

   [PunRPC]
   public void OpenDoor()
   {
      _animator.Play(_animationOpenName,0,0);
   }
   
   [PunRPC]
   public void Disable()
   {
      _opened = true;
      _animator.Play(_animationCloseName,0,0);
      StartCoroutine(Cooldown());
   }

   [PunRPC]
   public void SetPosition(Vector3 vector3, Quaternion quaternionSerializable)
   {
      transform.position = vector3;
      transform.rotation = quaternionSerializable;
   }

   private IEnumerator Cooldown()
   {
      yield return new WaitForSeconds(openingTime);
      PhotonView1.RPC("OpenDoor", RpcTarget.All);
      yield return new WaitForSeconds(cooldown - openingTime);
      PhotonView1.RPC("Enable", RpcTarget.All);
   }
}