using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody _rb = default;
    [SerializeField]float _jumpForce = default;
    bool _canJump = default;
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
