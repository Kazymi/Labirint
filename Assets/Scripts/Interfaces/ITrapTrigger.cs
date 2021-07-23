using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface ITrapSetting
{
   PhotonView PhotonView { get; set; }
   void StartAction();

   void Enable();

   void Disable();

   void SetPosition(Vector3 vector3,Quaternion quaternionSerializable);
}
