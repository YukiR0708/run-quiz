using UnityEngine;
using UniRx;
using System;

/// <summary> ���l��ێ�����N���X </summary>
public class PlayerValues : SingletonMonoBehaviour<PlayerValues>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField, Tooltip("�W�����v��")] float _jumpForce = default;
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

    /// <summary> PlayerCondition�Ƀt���O���Z�b�g���� </summary>
    public void SetFlag(PlayerCondition flag) { _watchPlayerFlag.Value |= flag; }

    /// <summary> PlayerCondition�Ƀt���O���O�� </summary>
    public void UnsetFlag(PlayerCondition flag) { _watchPlayerFlag.Value &= ~flag; }

    /// <summary> PlayerCondition��flag���܂܂�Ă��邩�ǂ��� </summary>
    public bool HasFlag(PlayerCondition flag) { return _watchPlayerFlag.Value.HasFlag(flag); }


    /// <summary> �N�C�Y��Player�̑I���𔻒肷��enum </summary>
    public enum PlayerSelect
    {
        None,
        A,    //�I�����FRed
        B,   //�I�����FBlue
    }
    [SerializeField, Tooltip("���݂̑I��")] PlayerSelect _pSelect = PlayerSelect.None;
    public PlayerSelect PSelect { get => _pSelect; set => _pSelect = value; }

   [Tooltip("���݂̃X�R�A")] int _score = 0;
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
