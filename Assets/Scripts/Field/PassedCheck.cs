using UnityEngine;

/// <summary> Playerが通過したら検知するクラス  </summary>
public class PassedCheck : MonoBehaviour
{
    MapGenerator _mapGene = default;
    GameManager _gm = default;
    PlayerValues _pv = default;
    private void Start()
    {
        _mapGene = MapGenerator.Instance;
        _pv = PlayerValues.Instance;
        _gm = GameManager.Instance;
    }

    private void Update()
    {
        //ボーダーの通過を検知
        if (_gm.NowMode == GameManager.GameMode.InGame
                && _pv.HasFlag(PlayerValues.PlayerCondition.Run))
        {
            if (transform.position.z <= _mapGene.BorderPos.z)
            {
                    MapGenerator.Instance.DestroyAndSpawn(this.gameObject);
            }
        }

    }
}
