using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Player���ʉ߂����猟�m����N���X  </summary>
public class PassedCheck : MonoBehaviour
{
    MapGenerator _mapGene = default;
    [Tooltip("Player�̍��W")] Vector3 _pPos = default;
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
