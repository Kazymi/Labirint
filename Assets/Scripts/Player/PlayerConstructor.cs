using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerConstructor : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private GameObject camera;
    
    private PlayerPunch _playerPunch;
    private AnimationControl _animationControl;
    private Movement _movement;
    public PlayerPunch PlayerPunch => _playerPunch;

    private void Start()
    {
        var pv = GetComponent<PhotonView>();
        var inputHandler = ServiceLocator.GetService<InputHandler>();
        _playerPunch = GetComponent<PlayerPunch>();
        if (pv.IsMine)
        {
            _animationControl = gameObject.AddComponent<AnimationControl>();
            _animationControl.Initialized(inputHandler,animator);
            _movement = gameObject.AddComponent<Movement>();
            _movement.Initialize(inputHandler,rotateTransform);
            gameObject.AddComponent<PlayerTrigger>();
            gameObject.AddComponent<PlayerHealth>().Initialize(_movement,_animationControl);
        } else Destroy(camera);
    }
}