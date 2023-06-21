using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;
using UniRx;
using System.Net.Http;

[RequireComponent(typeof(ChatGPTClient))]
public class QuestionGenerator : SingletonMonoBehaviour<QuestionGenerator>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }

    ChatGPTClient _chatGPTClient = default;
    PlayerValues _pv = default;
    MapGenerator _mapGene = default;
    [SerializeField, Tooltip("��蕶�̃e�L�X�g")] Text _text = default;
    [SerializeField, Tooltip("��Փx�̃e�L�X�g")] Text _levelText = default;
    [SerializeField, Tooltip("��蕶�̕\���ɂ�����b��")] float _textTime = 2.0f;
    [SerializeField, Tooltip("�o��W������")] List<string> genres = new();
    [SerializeField, Tooltip("�̃}�e���A��")] Material _blueMate = default;
    [SerializeField, Tooltip("�Ԃ̃}�e���A��")] Material _redMate = default;
    [SerializeField, Tooltip("���̃}�e���A��")] Material _purpleMate = default;
    [SerializeField, Tooltip("���o��܂ł̃t�B�[���h�̌�")] int _interval = 0;
    [Tooltip("��")] string _answer = "";
    public string Answer => _answer;
    [Tooltip("�o���Փx")] int _level = 0;
    public int Level => _level;

    void Start()
    {
        ReStart();  
    }

    public async Task Generate(string genre, int level)
    {
        _mapGene.ChangeMaterials(_blueMate, _redMate);
        _text.text = "";
        string[] texts =
         {
            "��蕶�F",
            "A�F",
            "B�F",
            "�q���g�F",
            "�𓚁F"
        };

        //���𐶐��A���͂���texts�Ɋi�[
        try
        {
            await _chatGPTClient.SendRequestAsync(level, genre,
               (r) =>
               {
                   for (int i = 0; i < texts.Length; i++) texts[i] += r[i];
                   _answer = r[texts.Length - 1];
               });

            string temp = "";
            for (int i = 0; i < level; i++)
            {
                temp += "��";
            }
            for (int i = 0; i < 5 - level; i++)    //5�͓�Փx�̍ő�l
            {
                temp += "��";
            }

            _levelText.DOText($"��Փx�F{temp}", 1.0f);

            var text = "";
            for (int i = 0; i < texts.Length - 1; i++) //�𓚂͂܂��\�����Ȃ��̂�-1
            {
                if (i == 1) text += $"<size=70><color=#dc143c>{texts[i]}</color></size>\n";
                else if (i == 2) text += $"<size=70><color=#4169e1>{texts[i]}</color></size>\n";
                else text += $"<color=#ffffff>{texts[i]}</color>\n";
            }

            _text.DOText(text, _textTime).OnComplete(() =>
            {
                Debug.Log(texts[4]);
                _pv.UnsetFlag(PlayerValues.PlayerCondition.Response);
            });
        }
        catch (HttpRequestException ex)
        {
            Debug.Log(ex.Message);
            Debug.Log("ChatGPT�̒ʐM�ŃG���[���������܂����B�����Ĕ��s���܂�");
            _pv.SetFlag(PlayerValues.PlayerCondition.Response);
        }

    }

    public override void ReStart()
    {
        _text.text = "";
        _levelText.text = "";
        _mapGene = MapGenerator.Instance; ;
        _chatGPTClient = GetComponent<ChatGPTClient>();
        _pv = PlayerValues.Instance;

        //PlayerCondition�̕ύX�ʒm���w��
        _pv.OnBitFlagVariableChanged.Subscribe(async newValue =>
        {
            Debug.Log("�t���O�ύX" + newValue);

            if (newValue.HasFlag(PlayerValues.PlayerCondition.Response))
            {
                _levelText.text = "";
                Debug.Log("���𐶐����܂�");
                string genre = genres[Random.Range(0, genres.Count)];
                _level = Random.Range(1, 6);
                await Generate(genre, _level);
            }
            //PlayerValues�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
        }).AddTo(_pv);

        _mapGene.OnPassdeCountChanged.Subscribe(newValue =>
        {
            if (_pv.HasFlag(PlayerValues.PlayerCondition.Run)&&
            !(_pv.  HasFlag(PlayerValues.PlayerCondition.Fell)))
            {
                if (newValue > 0 && newValue % (_interval * 2) == 0)
                {
                    _pv.SetFlag(PlayerValues.PlayerCondition.AnswerCheck);
                    _text.text = "";
                    _levelText.text = "";
                    _mapGene.ChangeMaterials(_purpleMate, _purpleMate);
                }
                else if (newValue > 0 && newValue % _interval == 0)
                {
                    _pv.SetFlag(PlayerValues.PlayerCondition.Response);
                }
            }
        }
            //MapGenerator�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
            ).AddTo(_mapGene);
    }
}
