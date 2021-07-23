using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerConstructor : MonoBehaviour
{
    [SerializeField] private Movenment movenment;
    [SerializeField] private GameObject camera;
    [SerializeField] private AnimationControl animationControl;
    [SerializeField] private PlayerTrigger playerTrigger;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private PlayerPunch playerPunch;


    public PlayerPunch PlayerPunch => playerPunch;
    private void Start()
    {
        var pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            var inputHandler = ServiceLocator.GetService<InputHandler>();
            var gameManager = ServiceLocator.GetService<GameManager>();
            movenment.Initialize(inputHandler);
            animationControl.Initialized(inputHandler);
            inputHandler.Initialize(playerTrigger,playerPunch,movenment);
            playerHealth.Initialize(movenment, animationControl);
            playerStatistics.Initialize(gameManager);
        }
        else
        {
            Destroy(playerStatistics);
            Destroy(movenment);
            Destroy(camera);
            Destroy(animationControl);
            Destroy(playerTrigger);
            Destroy(playerHealth);
        }
    }
}