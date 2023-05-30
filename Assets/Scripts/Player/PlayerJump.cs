using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : SingletonMonoBehaviour<PlayerJump>
{
    Rigidbody _rb = default;
    [SerializeField]float _jumpForce = default;
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }
    bool _canJump = default;

    protected override bool _dontDestroyOnLoad { get { return true; } }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _canJump = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            _canJump = false;
            _rb.AddForce(Vector3.up * _jumpForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<FieldRotate>(out var field))
        {
            _canJump = true;
        }
    }
}
