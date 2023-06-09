using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Playerが通過したら検知するクラス  </summary>
public class PassedCheck : MonoBehaviour
{
    MapGenerator _mapGene = default;
    [Tooltip("Playerの座標")] Vector3 _pPos = default;
    private void Start()
    {
        _mapGene = MapGenerator.Instance;
        _pPos = PlayerValues.Instance.gameObject.transform.position;
    }

    private void Update()
    {

        if (GameManager.Instance.NowMode == GameManager.GameMode.InGame
                && PlayerValues.Instance.NowCondition == PlayerValues.PlayerCondition.Run)
        {
            if (transform.position.z <= _mapGene.BorderPos.z)
            {
                MapGenerator.Instance.DestroyAndSpawn(this.gameObject);
            }
        }

    }
}
