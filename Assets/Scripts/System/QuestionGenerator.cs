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
    [SerializeField, Tooltip("問題文のテキスト")] Text _text = default;
    [SerializeField, Tooltip("難易度のテキスト")] Text _levelText = default;
    [SerializeField, Tooltip("問題文の表示にかかる秒数")] float _textTime = 2.0f;
    [SerializeField, Tooltip("出題ジャンル")] List<string> genres = new();
    [SerializeField, Tooltip("青のマテリアル")] Material _blueMate = default;
    [SerializeField, Tooltip("赤のマテリアル")] Material _redMate = default;
    [SerializeField, Tooltip("紫のマテリアル")] Material _purpleMate = default;
    [SerializeField, Tooltip("問題出題までのフィールドの個数")] int _interval = 0;
    [Tooltip("解答")] string _answer = "";
    public string Answer => _answer;
    [Tooltip("出題難易度")] int _level = 0;
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
            "問題文：",
            "A：",
            "B：",
            "ヒント：",
            "解答："
        };

        //問題を生成、分析してtextsに格納
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
                temp += "★";
            }
            for (int i = 0; i < 5 - level; i++)    //5は難易度の最大値
            {
                temp += "☆";
            }

            _levelText.DOText($"難易度：{temp}", 1.0f);

            var text = "";
            for (int i = 0; i < texts.Length - 1; i++) //解答はまだ表示しないので-1
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
            Debug.Log("ChatGPTの通信でエラーが発生しました。問題を再発行します");
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

        //PlayerConditionの変更通知を購読
        _pv.OnBitFlagVariableChanged.Subscribe(async newValue =>
        {
            Debug.Log("フラグ変更" + newValue);

            if (newValue.HasFlag(PlayerValues.PlayerCondition.Response))
            {
                _levelText.text = "";
                Debug.Log("問題を生成します");
                string genre = genres[Random.Range(0, genres.Count)];
                _level = Random.Range(1, 6);
                await Generate(genre, _level);
            }
            //PlayerValuesはシングルトンだが、念のためDispose（購読解除）しておく
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
            //MapGeneratorはシングルトンだが、念のためDispose（購読解除）しておく
            ).AddTo(_mapGene);
    }
}
