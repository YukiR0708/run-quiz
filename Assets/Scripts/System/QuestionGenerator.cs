using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ChatGPTClient))]
public class QuestionGenerator : SingletonMonoBehaviour<QuestionGenerator>
{
    protected override bool _dontDestroyOnLoad { get { return true; } }
    [SerializeField] Text _question = default;
    [SerializeField] Text _level = default;
    [SerializeField] float _textTime = 2.0f;
    ChatGPTClient _chatGPTClient = default;
    void Start()
    {
        _chatGPTClient = GetComponent<ChatGPTClient>();
        Generate("テスト", 3);
    }

    private void Update()
    {
      //UniRXでPlayerValuesを監視→Responseになった瞬間に出題する
    }

    public void Generate(string genre, int level)
    {
        string question = "問題文：";
        StartCoroutine(_chatGPTClient.SendRequest(level, genre, (s) => question += s));

string temp = "";
        for (int i = 0; i < level; i++)
        {
            temp += "★";
        }
        for(int i = 0; i < 5-level; i++)    //5は難易度の最大値
        {
            temp += "☆";
        }

        _level.text = $"難易度：{temp}";
        //ChatGPTからの出力をansに入れる
        _question.DOText(question, _textTime);
    }
}
