using UnityEngine;
using UniRx;
using System;

/// <summary> 数値を保持するクラス </summary>
public class PlayerValues : SingletonMonoBehaviour<PlayerValues>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField, Tooltip("ジャンプ力")] float _jumpForce = default;
    public float JumpForce => _jumpForce;
 
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


    /// <summary> クイズでPlayerの選択を判定するenum </summary>
    public enum PlayerSelect
    {
        None,
        A,    //選択肢：Red
        B,   //選択肢：Blue
    }
    [SerializeField, Tooltip("現在の選択")] PlayerSelect _pSelect = PlayerSelect.None;
    public PlayerSelect PSelect { get => _pSelect; set => _pSelect = value; }

   [Tooltip("現在のスコア")] int _score = 0;
    public int Score { get => _score; set => _score = value; }

    public override void ReStart()
    {
        _score = 0;
        _pSelect = PlayerSelect.None;
        _watchPlayerFlag.Dispose();
        _watchPlayerFlag = new ReactiveProperty<PlayerCondition>();
        SetFlag(PlayerCondition.Run);
    }
}
