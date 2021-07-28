using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PhotonView))]
public class PlayerPunch : MonoBehaviour
{
    [SerializeField] private Transform punchTransform;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private float kickStrength = 3f;

    private PhotonView photonViewMain;

    private bool _punched;
    private CharacterController character;
    private Vector3 _impact = Vector3.zero;
    private InputHandler _inputHandler;

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PunchAction += Punch;
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.PunchAction -= Punch;
        }
    }
    
    private void Awake()
    {
        character = GetComponent<CharacterController>();
        photonViewMain = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _inputHandler = ServiceLocator.GetService<InputHandler>();
        _inputHandler.PunchAction += Punch;
    }

    private void Update()
    {
        _impact = new Vector3(_impact.x, 0, _impact.z);
        if (_impact.magnitude > 0.2) character.Move(_impact * Time.deltaTime);
        {
            _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
        }
    }

    public void Punch()
    {
        if (Physics.Raycast(punchTransform.position, punchTransform.forward, out RaycastHit raycastHit, maxDistance))
        {
            var constructor = raycastHit.transform.GetComponent<PlayerConstructor>();
            if (constructor)
            {
                var i = constructor.PlayerPunch;
                i.photonViewMain.RPC(RPCEventType.GetPunch, RpcTarget.All, raycastHit.point, kickStrength);
            }
        }
    }

    [PunRPC]
    public void GetPunch(Vector3 point, float kickStrength)
    {
        var dir = (transform.position - point).normalized;
        if (dir.y < 0) dir.y = -dir.y;
        {
            _impact += dir.normalized * kickStrength;
        }
    }
}