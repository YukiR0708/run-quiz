using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>　入力を受け取ってフィールドを回転させるクラス </summary>
public class FieldRotate : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 0f;
    public float RotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }

    private void Update()
    {
        if (GameManager.Instance.NowMode == GameManager.GameMode.InGame
    && PlayerValues.Instance.NowCondition == PlayerValues.PlayerCondition.Run)
        {
            var hInput = Input.GetAxisRaw("Horizontal");
            if (hInput < 0f)
            {
                transform.rotation *= Quaternion.AngleAxis(_rotateSpeed, Vector3.up);
            }
            else if (0f < hInput)
            {
                transform.rotation *= Quaternion.AngleAxis(-_rotateSpeed, Vector3.up);
            }

        }

    }


}
