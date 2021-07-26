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
    private Movenment _movement;

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
            _animationControl.Initialized(inputHandler, animator);
            _movement = gameObject.AddComponent<Movenment>();
            _movement.Initialize(inputHandler, rotateTransform);
            _playerTrigger = gameObject.AddComponent<PlayerTrigger>();
            inputHandler.Initialize(_playerTrigger, _playerPunch, _movement);
            gameObject.AddComponent<PlayerHealth>().Initialize(_movement, _animationControl);
        }
        else Destroy(camera);
    }
}