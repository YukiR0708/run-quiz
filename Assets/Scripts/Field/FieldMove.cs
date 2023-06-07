using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMove : MonoBehaviour
{
    [SerializeField, Tooltip("フィールドの移動速度")] float _speed = 0f;
    public float Speed { get => _speed; set => _speed = value; }

    private void Update()
    {
        if(GameManager.Instance.NowMode ==  GameManager.GameMode.InGame
            && PlayerValues.Instance.NowCondition == PlayerValues.PlayerCondition.Run)
        {
            transform.position -= new Vector3(0f, 0f, _speed * Time.deltaTime);
        }
    }
}
