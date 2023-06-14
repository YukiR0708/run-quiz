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
    [SerializeField, Tooltip("問題出題までのフィールド個数")] int _interval = 0;

    void Start()
    {
        _mapGene = MapGenerator.Instance; ;
        _chatGPTClient = GetComponent<ChatGPTClient>();
        _pv = PlayerValues.Instance;

        //PlayerConditionの変更通知を購読
        _pv.OnBitFlagVariableChanged.Subscribe(async newValue =>
        {
            Debug.Log("フラグ変更" + newValue);

            if (newValue.HasFlag(PlayerValues.PlayerCondition.Response))
            {
                Debug.Log("問題を生成します");
                _text.text = "";
                string genre = genres[Random.Range(0, genres.Count)];
                await Generate(genre, Random.Range(1, 6));
                _pv.UnsetFlag(PlayerValues.PlayerCondition.Response);
                _pv.SetFlag(PlayerValues.PlayerCondition.AnswerCheck);
            }

            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //正誤判定してスコア加算する
            }

            //PlayerValuesはシングルトンだが、念のためDispose（購読解除）しておく
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
            //MapGeneratorはシングルトンだが、念のためDispose（購読解除）しておく
            ).AddTo(_mapGene);
    }

    public async Task Generate(string genre, int level)
    {
        string[] texts =
         {
            "問題文：",
            "A：",
            "B：",
            "ヒント：",
            "解答："
        };

        //問題を生成、分析してtextsに格納
        await _chatGPTClient.SendRequestAsync(level, genre,
           (r) => { for (int i = 0; i < texts.Length; i++) texts[i] += r[i]; });

        string temp = "";
        for (int i = 0; i < level; i++)
        {
            temp += "★";
        }
        for (int i = 0; i < 5 - level; i++)    //5は難易度の最大値
        {
            temp += "☆";
        }

        _level.DOText($"難易度：{temp}", 1.0f);

        var text = "";
        for (int i = 0; i < texts.Length - 1; i++) //解答はまだ表示しないので-1
        {
            text += texts[i] + "\n";
        }

        _text.DOText(text, _textTime)/*.OnComplete(() => _text.text = texts[4])*/;
    }
}
