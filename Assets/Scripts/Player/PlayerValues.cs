using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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
    [SerializeField]
    private ReactiveProperty<PlayerCondition> _watchPlayerFlag = new ReactiveProperty<PlayerCondition>();
    public IObservable<PlayerCondition> OnBitFlagVariableChanged => _watchPlayerFlag;

    /// <summary> PlayerConditionにフラグをセットする </summary>
    public void SetFlag(PlayerCondition flag) { _watchPlayerFlag.Value |= flag; }

    /// <summary> PlayerConditionにフラグを外す </summary>
    public void UnsetFlag(PlayerCondition flag) { _watchPlayerFlag.Value &= ~flag; }

    /// <summary> PlayerConditionにflagが含まれているかどうか </summary>
    public bool HasFlag(PlayerCondition flag) { return _watchPlayerFlag.Value.HasFlag(flag); }




    public enum SelectColor
    {
        None,
        Blue,
        Red,
    }
    [SerializeField] SelectColor _nowColor = SelectColor.None;
    public SelectColor NowColor { get => _nowColor; set => _nowColor = value; }

    static int _score = 0;
    public int Score { get => _score; }

    void Start()
    {

    }

    void Update()
    {

    }
}
