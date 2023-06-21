using UnityEngine;

/// <summary>�@���͂��󂯎���ăt�B�[���h����]������N���X </summary>
public class FieldRotate : MonoBehaviour
{
    [SerializeField, Tooltip("��]���x")] float _rotateSpeed = 0f;
    GameManager _gm = default;
    PlayerValues _pv = default;
    //public float RotateSpeed { get => _rotateSpeed; set => _rotateSpeed = value; }


    private void Start()
    {
        _gm = GameManager.Instance;
        _pv = PlayerValues.Instance;
    }
    private void Update()
    {
        if (_gm.NowMode == GameManager.GameMode.InGame
    && _pv.HasFlag(PlayerValues.PlayerCondition.Run))
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
