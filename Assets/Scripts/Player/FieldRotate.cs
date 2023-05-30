using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�@���͂��󂯎���ăt�B�[���h����]������N���X </summary>
public class FieldRotate : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 0f;
    public float RotateSpeed { get => _rotateSpeed; set=> _rotateSpeed = value; }

    private void Update()
    {
        var hInput = Input.GetAxisRaw("Horizontal");
        if(hInput < 0f)
        {
            transform.Rotate(Vector3.forward, -_rotateSpeed, Space.World);
        }
        else if(0f < hInput)
        {
            transform.Rotate(Vector3.forward, _rotateSpeed, Space.World);
        }
    }


}
