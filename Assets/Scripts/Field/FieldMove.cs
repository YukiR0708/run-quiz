using UnityEngine;

public class FieldMove : MonoBehaviour
{
    [SerializeField, Tooltip("フィールドの移動速度")] float _speed = 0f;
    //public float Speed { get => _speed; set => _speed = value; }
    GameManager _gm = default;
    PlayerValues _pv = default;

    private void Start()
    {
        _gm = GameManager.Instance;
        _pv = PlayerValues.Instance;
    }
    private void Update()
    {
        if(_gm.NowMode == GameManager.GameMode.InGame
            && _pv.HasFlag( PlayerValues.PlayerCondition.Run))
        {
            transform.position -= new Vector3(0f, 0f, _speed * Time.deltaTime);
        }
    }
}
