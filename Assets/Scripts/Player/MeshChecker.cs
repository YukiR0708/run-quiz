using UnityEngine;

/// <summary> Player���牺�����̃t�B�[���h�̃��b�V���𒲂ׂ�  </summary>
public class MeshChecker : MonoBehaviour
{
    Rigidbody _rb = default;
    PlayerValues _pv = default;
    PlayerJump _pj = default;
    [SerializeField] float _downForce = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pv = PlayerValues.Instance;
        _pj = GetComponent<PlayerJump>();
    }

    /// <summary> �����蔻�肪�L���b�V������邹���H�ŗ����ł��Ȃ��̂ŁA
    /// Trigger���������牺�� AddForce���ė��Ƃ�</summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<FieldMove>(out var field))
        {
            if (_pj.Canjamp)
            {
                _rb.AddForce(Vector3.down * _downForce);
            }
        }
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        //Player�̉������Ƀt�B�[���h������A����Player���񓚒���������
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            if (_pv.HasFlag(PlayerValues.PlayerCondition.Response)
                && hit.collider.gameObject.TryGetComponent<FieldMove>(out var field))
            {
                var fieldRot = field.gameObject.transform.localEulerAngles.x;
                //�t�B�[���h��x����]�p�x�ɂ����Player�̑I�������F�𔻒肷��
                if (fieldRot < 180f) _pv.PSelect = PlayerValues.PlayerSelect.B;
                else _pv.PSelect = PlayerValues.PlayerSelect.A;
            }
        }
    }
}
