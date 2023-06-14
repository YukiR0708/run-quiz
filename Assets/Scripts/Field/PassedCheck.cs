using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Playerが通過したら検知するクラス  </summary>
public class PassedCheck : MonoBehaviour
{
    MapGenerator _mapGene = default;
    [Tooltip("Playerの座標")] Vector3 _pPos = default;
    GameManager _gm = default;
    PlayerValues _pv = default;
    private void Start()
    {
        _mapGene = MapGenerator.Instance;
        _pv = PlayerValues.Instance;
        _gm = GameManager.Instance;
        _pPos = _pv.gameObject.transform.position;
    }

    private void Update()
    {

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
