using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerHealth : MonoBehaviour
{

    private AnimationControl _playerAnimator;
    private PhotonView _pv;
    private PlayerManager _playerManager;
    private Movement _movement;
    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        _playerManager = PhotonView.Find((int)_pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    public void Initialize(Movement movement, AnimationControl animationControl)
    {
        _movement = movement;
        _playerAnimator = animationControl;
    }
    
    public void Death()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        Destroy(_movement);
        _playerAnimator.Die();
        yield return new WaitForSeconds(4f);
        _playerManager.Die();
    }
}
