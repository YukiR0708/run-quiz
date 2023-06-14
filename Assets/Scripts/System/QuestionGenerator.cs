using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;
using UniRx;

[RequireComponent(typeof(ChatGPTClient))]
public class QuestionGenerator : SingletonMonoBehaviour<QuestionGenerator>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField] Text _text = default;
    [SerializeField] Text _level = default;
    [SerializeField] float _textTime = 2.0f;
    ChatGPTClient _chatGPTClient = default;
    PlayerValues _pv = default;
    [SerializeField] List<string> genres = new();
    MapGenerator _mapGene = default;
    [SerializeField, Tooltip("���o��܂ł̃t�B�[���h��")] int _interval = 0;

    void Start()
    {
        _mapGene = MapGenerator.Instance; ;
        _chatGPTClient = GetComponent<ChatGPTClient>();
        _pv = PlayerValues.Instance;

        //PlayerCondition�̕ύX�ʒm���w��
        _pv.OnBitFlagVariableChanged.Subscribe(async newValue =>
        {
            Debug.Log("�t���O�ύX" + newValue);

            if (newValue.HasFlag(PlayerValues.PlayerCondition.Response))
            {
                Debug.Log("���𐶐����܂�");
                _text.text = "";
                string genre = genres[Random.Range(0, genres.Count)];
                await Generate(genre, Random.Range(1, 6));
                _pv.UnsetFlag(PlayerValues.PlayerCondition.Response);
                _pv.SetFlag(PlayerValues.PlayerCondition.AnswerCheck);
            }

            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //���딻�肵�ăX�R�A���Z����
            }

            //PlayerValues�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
        }).AddTo(_pv);

        _mapGene.OnPassdeCountChanged.Subscribe(newValue =>
        {
            if (_pv.HasFlag(PlayerValues.PlayerCondition.Run))
            {
                if (newValue > 0 && newValue % _interval == 0)
                {
                    _pv.SetFlag(PlayerValues.PlayerCondition.Response);
                }
            }
        }
            //MapGenerator�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
            ).AddTo(_mapGene);
    }

    public async Task Generate(string genre, int level)
    {
        string[] texts =
         {
            "��蕶�F",
            "A�F",
            "B�F",
            "�q���g�F",
            "�𓚁F"
        };

        //���𐶐��A���͂���texts�Ɋi�[
        await _chatGPTClient.SendRequestAsync(level, genre,
           (r) => { for (int i = 0; i < texts.Length; i++) texts[i] += r[i]; });

        string temp = "";
        for (int i = 0; i < level; i++)
        {
            temp += "��";
        }
        for (int i = 0; i < 5 - level; i++)    //5�͓�Փx�̍ő�l
        {
            temp += "��";
        }

        _level.DOText($"��Փx�F{temp}", 1.0f);

        var text = "";
        for (int i = 0; i < texts.Length - 1; i++) //�𓚂͂܂��\�����Ȃ��̂�-1
        {
            text += texts[i] + "\n";
        }

        _text.DOText(text, _textTime)/*.OnComplete(() => _text.text = texts[4])*/;
    }
}
