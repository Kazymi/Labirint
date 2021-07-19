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
   public override void OnEnable()
   {
      _animator = GetComponent<Animator>();
   }

   public void StartAction()
   {
      if(_opened) return;
      photonView.RPC("Disable", RpcTarget.All);
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

   IEnumerator Cooldown()
   {
      yield return new WaitForSeconds(openingTime);
      photonView.RPC("OpenDoor", RpcTarget.All);
      yield return new WaitForSeconds(cooldown - openingTime);
      photonView.RPC("Enable", RpcTarget.All);
   }
}
