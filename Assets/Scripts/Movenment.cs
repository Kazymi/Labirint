using Photon.Pun;
using UnityEngine;

public class Movenment : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float gravity = 20f;

    private PhotonView _photonView;
    private Transform _rotateGameObject;
    private InputHandler _inputHandler;
    private bool _isGround;
    private Vector3 _moveDir = Vector3.zero;
    private float _speedBoost = 0f;
    private CharacterController _control;

    private void Start()
    {
        _control = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _isGround = _control.isGrounded;
        Move();
    }

    public void Initialize(InputHandler inputHandler,Transform rotateTransform)
    {
        _inputHandler = inputHandler;
        _rotateGameObject = rotateTransform;
    }

    private void Move()
    {
        if (_isGround)
        {
            var direction = _inputHandler.MoveDirection();
            _moveDir = new Vector3(direction.x, 0, direction.y).normalized;
            _moveDir *= speed;
            if (Input.GetKey(KeyCode.Space))
            {
                _moveDir.y = jumpForce;
            }
        }
        if (_inputHandler.MoveDirection() != Vector2.zero)
        {
            var i = new Vector3(0,
                Mathf.Atan2(_inputHandler.MoveDirection().x, _inputHandler.MoveDirection().y) * 180 / Mathf.PI, 0);
            _rotateGameObject.rotation = Quaternion.Lerp(_rotateGameObject.rotation, Quaternion.Euler(i), Time.deltaTime * 4.0f);
        }
        _moveDir.y -= gravity * Time.deltaTime;
        _control.Move(_moveDir * Time.deltaTime);
    }
}