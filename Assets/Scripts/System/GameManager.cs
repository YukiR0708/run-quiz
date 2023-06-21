using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    PlayerValues _pv = default;
    [SerializeField, Tooltip("ボーナススコア")] float _bonusScore = 3600f;
    [SerializeField, Tooltip("問題数の最大")] int _qMax = default;
    [SerializeField] Text _qNumText = default;
    [Tooltip("現在の問題数")] int _qCount = 0;
    [SerializeField, Tooltip("リザルトパネル")] GameObject _resultPanel = default;
    [SerializeField, Tooltip("スタートパネル")] GameObject _startPanel = default;
    [Tooltip("最終スコア")] int _finalScore = 0;
    [SerializeField, Tooltip("スコア")] Text[] scoreTexts = new Text[4];
    [SerializeField, Tooltip("スコア判定の条件")] int[] _rateCheck = new int[3];
    [Tooltip("今回のプレイの時間")]float _nowTime = 0;

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
        _qNumText.text = $"残り{_qMax - _qCount}問";
        //出題数が最大値を超えたら
        if (_qCount >= _qMax)
        {
            _pv.SetFlag(PlayerValues.PlayerCondition.Stop);
            _nowMode = GameMode.Result;
            CalcFinalScore(scoreTexts);
            _resultPanel.SetActive(true);
            _qCount = 0;
        }
    }

    /// <summary> スコアを計算してテキストにセットする </summary>
    /// <param name="texts"></param>
    private void CalcFinalScore(Text[] texts)
    {
        //初期ボーナス値から経過時間を引く
        _bonusScore -= (Time.time - _nowTime);
        //スコアをセットする
        texts[0].text = "獲得スコア：" + _pv.Score.ToString("00000");
        texts[1].text = "ボーナススコア：" + ((int)_bonusScore).ToString("00000");
        _finalScore = (_pv.Score + (int)_bonusScore);
        texts[2].text = "最終スコア：" + _finalScore.ToString("00000");

        //最終スコアに応じてレート判定
        if (_finalScore > _rateCheck[0]) texts[3].text = "S";
        else if (_finalScore > _rateCheck[1]) texts[3].text = "A";
        else if (_finalScore > _rateCheck[2]) texts[3].text = "B";
        else texts[3].text = "C";

    }

    /// <summary> ゲームをリスタートするところから呼ぶ </summary>
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
