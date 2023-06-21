using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    PlayerValues _pv = default;
    [SerializeField, Tooltip("�{�[�i�X�X�R�A")] float _bonusScore = 3600f;
    [SerializeField, Tooltip("��萔�̍ő�")] int _qMax = default;
    [SerializeField] Text _qNumText = default;
    [Tooltip("���݂̖�萔")] int _qCount = 0;
    [SerializeField, Tooltip("���U���g�p�l��")] GameObject _resultPanel = default;
    [SerializeField, Tooltip("�X�^�[�g�p�l��")] GameObject _startPanel = default;
    [Tooltip("�ŏI�X�R�A")] int _finalScore = 0;
    [SerializeField, Tooltip("�X�R�A")] Text[] scoreTexts = new Text[4];
    [SerializeField, Tooltip("�X�R�A����̏���")] int[] _rateCheck = new int[3];
    [Tooltip("����̃v���C�̎���")]float _nowTime = 0;

    public int QCount { get => _qCount; set => _qCount = value; }
    public enum GameMode
    {
        Manual,
        InGame,
        //Pause,
        //InPause,
        Result,
        None,
    }
    [SerializeField] GameMode _nowMode = GameMode.None;
    public GameMode NowMode { get => _nowMode; set => _nowMode = value; }

    protected override bool _dontDestroyOnLoad { get { return true; } }

    private void Start()
    {
        ReStart();
    }
    void Update()
    {
        _qNumText.text = $"�c��{_qMax - _qCount}��";
        //�o�萔���ő�l�𒴂�����
        if (_qCount >= _qMax)
        {
            _pv.SetFlag(PlayerValues.PlayerCondition.Stop);
            _nowMode = GameMode.Result;
            CalcFinalScore(scoreTexts);
            _resultPanel.SetActive(true);
            _qCount = 0;
        }
    }

    /// <summary> �X�R�A���v�Z���ăe�L�X�g�ɃZ�b�g���� </summary>
    /// <param name="texts"></param>
    private void CalcFinalScore(Text[] texts)
    {
        //�����{�[�i�X�l����o�ߎ��Ԃ�����
        _bonusScore -= (Time.time - _nowTime);
        //�X�R�A���Z�b�g����
        texts[0].text = "�l���X�R�A�F" + _pv.Score.ToString("00000");
        texts[1].text = "�{�[�i�X�X�R�A�F" + ((int)_bonusScore).ToString("00000");
        _finalScore = (_pv.Score + (int)_bonusScore);
        texts[2].text = "�ŏI�X�R�A�F" + _finalScore.ToString("00000");

        //�ŏI�X�R�A�ɉ����ă��[�g����
        if (_finalScore > _rateCheck[0]) texts[3].text = "S";
        else if (_finalScore > _rateCheck[1]) texts[3].text = "A";
        else if (_finalScore > _rateCheck[2]) texts[3].text = "B";
        else texts[3].text = "C";

    }

    /// <summary> �Q�[�������X�^�[�g����Ƃ��납��Ă� </summary>
    public void ReStartGame()
    {
        PlayerJump.Instance.ReStart();
        PlayerValues.Instance.ReStart();
        MapGenerator.Instance.ReStart();
        QuestionGenerator.Instance.ReStart();   
        ResultChecker.Instance.ReStart();
        PlayerRespawn.Instance.ReStart();
        this.ReStart();
    }

    public override void ReStart()
    {
        _pv = PlayerValues.Instance;
        _nowTime = Time.time;
        _resultPanel.SetActive(false);
        _startPanel.SetActive(true);
    }
}
