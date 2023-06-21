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
        _scoreText.text = "�X�R�A�F" + _pv.Score.ToString("00000");

        //PlayerCondition�̕ύX�ʒm���w��
        _pv.OnBitFlagVariableChanged.Subscribe(newValue =>
        {
            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //���딻�肵�ăX�R�A���Z����
                if (_questionGene.Answer == _pv.PSelect.ToString())
                {
                    _pv.Score += (_questionGene.Level * 100);
                    _audioSource.PlayOneShot(_correct);
                }
                else
                {
                    _audioSource.PlayOneShot(_uncorrect);
                }
                _scoreText.text = "�X�R�A�F" + _pv.Score.ToString("00000");
                _pv.UnsetFlag(PlayerValues.PlayerCondition.AnswerCheck);
                _gm.QCount++;
            }


            //PlayerValues�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
        }).AddTo(_pv);


    }
}
