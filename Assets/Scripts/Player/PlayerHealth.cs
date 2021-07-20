using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerHealth : MonoBehaviour
{

    private AnimationControl _playerAnimator;
    private PhotonView _pv;
    private PlayerManager _playerManager;
    private Movenment _movenment;
    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        _playerManager = PhotonView.Find((int)_pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    public void Initialize(Movenment movenment, AnimationControl animationControl)
    {
        _movenment = movenment;
        _playerAnimator = animationControl;
    }
    
    public void Death()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        Destroy(_movenment);
        _playerAnimator.Die();
        yield return new WaitForSeconds(4f);
        _playerManager.Die();
    }
}
