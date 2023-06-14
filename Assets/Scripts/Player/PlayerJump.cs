using UnityEngine;

public class PlayerJump : SingletonMonoBehaviour<PlayerJump>
{
    Rigidbody _rb = default;
    [SerializeField]bool _canJump = default;
    GameManager _gm = default;
    PlayerValues _pv = default;
    protected override bool _dontDestroyOnLoad { get { return true; } }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _canJump = true;
        _gm = GameManager.Instance;
        _pv = PlayerValues.Instance;

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
}
