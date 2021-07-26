using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerConstructor : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform punchPosition;
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private GameObject camera;
    
    private PlayerPunch _playerPunch;
    private PlayerTrigger _playerTrigger;
    private AnimationControl _animationControl;
    private Movenment _movenment;
    public PlayerPunch PlayerPunch => _playerPunch;

    private void Start()
    {
        var pv = GetComponent<PhotonView>();
        var inputHandler = ServiceLocator.GetService<InputHandler>();
        _playerPunch = punchPosition.gameObject.AddComponent<PlayerPunch>();
        _playerPunch.Initialize(punchPosition);
        if (pv.IsMine)
        {
            _animationControl = gameObject.AddComponent<AnimationControl>();
            _animationControl.Initialized(inputHandler,animator);
            _movenment = gameObject.AddComponent<Movenment>();
            _movenment.Initialize(inputHandler,rotateTransform);
            _playerTrigger = gameObject.AddComponent<PlayerTrigger>();
            inputHandler.Initialize(_playerTrigger,_playerPunch,_movenment);
            gameObject.AddComponent<PlayerHealth>().Initialize(_movenment,_animationControl);
        } else Destroy(camera);
    }
}