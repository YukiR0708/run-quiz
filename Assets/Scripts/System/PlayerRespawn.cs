using System.Collections;
using UnityEngine;
using UniRx;

public class PlayerRespawn : SingletonMonoBehaviour<PlayerRespawn>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField]Vector3 _initialPos = default;
    PlayerValues _pv = default;
    [SerializeField] float _interval = default;
    void Start()
    {
        ReStart();
    }

    IEnumerator RespawnInterval()
    {
        yield return new WaitForSeconds(_interval);
        transform.position = _initialPos;
        _pv.UnsetFlag(PlayerValues.PlayerCondition.Fell);
    }


    private void Update()
    {
        if (!_pv.HasFlag(PlayerValues.PlayerCondition.Fell) &&
            transform.position.y < -20f)
        {
            _pv.SetFlag(PlayerValues.PlayerCondition.Fell);
        }
    }

    public override void ReStart()
    {
        _pv = PlayerValues.Instance;

        _pv.OnBitFlagVariableChanged.Subscribe(newValue =>
        {
            if (newValue.HasFlag(PlayerValues.PlayerCondition.Fell))
            {
                Debug.Log("ÉRÉãÅ[É`ÉìÇÃÇ‹Ç¶");
                StartCoroutine(RespawnInterval());
            }
        }).AddTo(_pv);
    }
}
