using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface TrapSetting
{
   PhotonView PhotonView1 { get; set; }
   void StartAction();

   void Enable();

   void Disable();

   void SetPosition(Vector3 vector3,Quaternion quaternionSerializable);
}
