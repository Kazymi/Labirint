using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Movenment : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private CharacterController control;
    [SerializeField] private Transform rotateGameObject;
    
    private PhotonView _photonView;
    private InputHandler _inputHandler;
    private bool _isGround = false;
    private Vector3 _moveDir = Vector3.zero;
    private float _speedBoost = 0f;

    private void Start()
    {
        control = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //TODO: should be in Update()
        _isGround = control.isGrounded;
        Move();
    }

    public void Initialize(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
    }

    private void Move()
    {
        if (_isGround)
        {
            var direction = _inputHandler.MoveDirection();
            _moveDir = new Vector3(direction.x, 0, direction.y);
            _moveDir = transform.TransformDirection(_moveDir);
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
            rotateGameObject.rotation = Quaternion.Lerp(rotateGameObject.rotation, Quaternion.Euler(i), Time.deltaTime * 4.0f);
        }
        _moveDir.y -= gravity * Time.deltaTime;
        control.Move(_moveDir * Time.deltaTime);
    }
}