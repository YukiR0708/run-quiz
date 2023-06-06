using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : SingletonMonoBehaviour<PlayerValues>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    public enum PlayerCondition
    {
        Stop,
        Run,
        Fell,
    }

    [SerializeField] PlayerCondition _nowCondition = PlayerCondition.Stop;
    [SerializeField] float _jumpForce = default;
    public PlayerCondition NowCondition { get => _nowCondition; set => _nowCondition = value; }
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }

    void Start()
    {

    }

    void Update()
    {

    }
}
