using UnityEngine;

public class PlayerJump : SingletonMonoBehaviour<PlayerJump>
{
    Rigidbody _rb = default;
    [SerializeField ,Tooltip("�W�����v�ł��邩�ǂ���")]bool _canJump = default;
    public bool Canjamp => _canJump;
    GameManager _gm = default;
    PlayerValues _pv = default;
    protected override bool _dontDestroyOnLoad { get { return true; } }

    void Start()
    {
        ReStart();
    }

    void Update()
    {
        if (_gm.NowMode == GameManager.GameMode.InGame
                && _pv.HasFlag(PlayerValues.PlayerCondition.Run))
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                _canJump = false;
                _rb.AddForce(Vector3.up * PlayerValues.Instance.JumpForce);
            }
        }
    }

    /// <summary> �ڒn�����؂�ւ��� </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (_gm.NowMode == GameManager.GameMode.InGame
                && _pv.HasFlag(PlayerValues.PlayerCondition.Run))
        {
            if (collision.gameObject.TryGetComponent<FieldMove>(out var field))
            {
                _canJump = true;
            }
        }
    }

    public override void ReStart()
    {
        _rb = GetComponent<Rigidbody>();
        _canJump = true;
        _gm = GameManager.Instance;
        _pv = PlayerValues.Instance;
    }
}
