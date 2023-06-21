using UnityEngine;

/// <summary> Playerから下方向のフィールドのメッシュを調べる  </summary>
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

    /// <summary> 当たり判定がキャッシュされるせい？で落下できないので、
    /// Triggerが抜けたら下に AddForceして落とす</summary>
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
        //Playerの下方向にフィールドがあり、かつPlayerが回答中だったら
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f))
        {
            if (_pv.HasFlag(PlayerValues.PlayerCondition.Response)
                && hit.collider.gameObject.TryGetComponent<FieldMove>(out var field))
            {
                var fieldRot = field.gameObject.transform.localEulerAngles.x;
                //フィールドのx軸回転角度によってPlayerの選択した色を判定する
                if (fieldRot < 180f) _pv.PSelect = PlayerValues.PlayerSelect.B;
                else _pv.PSelect = PlayerValues.PlayerSelect.A;
            }
        }
    }
}
