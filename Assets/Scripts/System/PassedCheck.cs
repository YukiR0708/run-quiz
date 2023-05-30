using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Player���ʉ߂����猟�m����N���X  </summary>
public class PassedCheck : MonoBehaviour
{
    GameObject _player = default;
    [Tooltip("Player�̍��W")] Vector3 _pPos = default;
    private void Start()
    {
        _player = PlayerJump.Instance.gameObject;
    }

    private void Update()
    {
        if(_player)
        {
            _pPos = _player.transform.position;
            var border = new Vector3(0, 0, transform.position.z + MapGenerator.Instance.Offset);
            if(_pPos .z > border.z)
            {
                MapGenerator.Instance.DestroyAndSpawn(this.gameObject);
            }
        }
    }
}
