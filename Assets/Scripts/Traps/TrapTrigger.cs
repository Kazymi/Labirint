using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TrapTrigger : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        var i = other.GetComponent<PlayerHealth>();
        if (i == false) return;
        i.Death();
    }
}