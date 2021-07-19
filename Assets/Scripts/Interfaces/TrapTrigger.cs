using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface TrapSetting
{
   void StartAction();

   [PunRPC]
   void Enable();
   
   [PunRPC]
   void Disable();
}
