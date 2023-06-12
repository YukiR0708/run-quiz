using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : SingletonMonoBehaviour<PlayerValues>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField] float _jumpForce = default;
    public float JumpForce { get => _jumpForce; set => _jumpForce = value; }

    [System.Flags]
    public enum PlayerCondition
    {
        Stop = 1 << 0,
        Run = 1 << 1,
        Response = 1 << 2,
        AnswerCheck = 1 << 3,
        Fell = 1 << 4,
    }

    [SerializeField] PlayerCondition _nowCondition = PlayerCondition.Stop;
    public PlayerCondition NowCondition { get => _nowCondition; set => _nowCondition = value; }

    public enum SelectColor
    {
        None,
        Blue,
        Red,
    }
    [SerializeField] SelectColor _nowColor = SelectColor.None;
    public SelectColor NowColor { get => _nowColor; set => _nowColor = value; }


    void Start()
    {

    }

    void Update()
    {

    }
}
