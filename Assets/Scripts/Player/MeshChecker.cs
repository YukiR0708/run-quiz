using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Player���牺�����̃t�B�[���h�̃��b�V���𒲂ׂ�  </summary>
public class MeshChecker : MonoBehaviour
{
    Rigidbody _rb = default;
    GameManager _gm = default;
    [SerializeField] float _downForce = default;
    [SerializeField] Transform _foot = default;
    PlayerValues _pv = default;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gm = GameManager.Instance;
        _pv = PlayerValues.Instance;
    }

    
    void FixedUpdate()
    {
        Ray ray = new Ray(_foot.position + new Vector3(0, 0, 10f), Vector3.down);
        //Player�̉������Ƀt�B�[���h������A����Player���񓚒���������
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            if (_pv.NowCondition.HasFlag(PlayerValues.PlayerCondition.Response)
                && hit.collider.gameObject.TryGetComponent<FieldMove>(out var field))
            {
                var fieldRot = field.gameObject.transform.localEulerAngles.x;
                //�t�B�[���h��x����]�p�x�ɂ����Player�̑I�������F�𔻒肷��
                if (fieldRot < 180f) _pv.NowColor = PlayerValues.SelectColor.Red;
                else _pv.NowColor = PlayerValues.SelectColor.Blue;
            }
            
        }
        else if(_pv.NowCondition.HasFlag(PlayerValues.PlayerCondition.Run))
        {
            //Player�̉������Ƀt�B�[���h���Ȃ��A�����s����������
            _pv.NowCondition &= ~PlayerValues.PlayerCondition.Run;
            _pv.NowCondition |= PlayerValues.PlayerCondition.Fell;
            _rb.AddForce(Vector3.down * _downForce);
        }
    }
}
