using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class ResultChecker : SingletonMonoBehaviour<ResultChecker>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    PlayerValues _pv = default;
    GameManager _gm = default;
    QuestionGenerator _questionGene = default;
    [SerializeField] Text _scoreText = default;
    [SerializeField] AudioSource _audioSource = default;
    [SerializeField] AudioClip _correct = default;
    [SerializeField] AudioClip _uncorrect = default;
    void Start()
    {
        ReStart();
    }

    public override void ReStart()
    {
        _pv = PlayerValues.Instance;
        _questionGene = QuestionGenerator.Instance;
        _gm = GameManager.Instance;
        _scoreText.text = "スコア：" + _pv.Score.ToString("00000");

        //PlayerConditionの変更通知を購読
        _pv.OnBitFlagVariableChanged.Subscribe(newValue =>
        {
            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //正誤判定してスコア加算する
                if (_questionGene.Answer == _pv.PSelect.ToString())
                {
                    _pv.Score += (_questionGene.Level * 100);
                    _audioSource.PlayOneShot(_correct);
                }
                else
                {
                    _audioSource.PlayOneShot(_uncorrect);
                }
                _scoreText.text = "スコア：" + _pv.Score.ToString("00000");
                _pv.UnsetFlag(PlayerValues.PlayerCondition.AnswerCheck);
                _gm.QCount++;
            }


            //PlayerValuesはシングルトンだが、念のためDispose（購読解除）しておく
        }).AddTo(_pv);


    }
}
