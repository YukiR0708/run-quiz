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
    [SerializeField] Text _hint = default;
    [SerializeField] float _textTime = 2.0f;
    ChatGPTClient _chatGPTClient = default;
    void Start()
    {
        _chatGPTClient = GetComponent<ChatGPTClient>();
        Generate("�X�|�[�c", 3);
    }

    private void Update()
    {
        //UniRX��PlayerValues���Ď���Response�ɂȂ����u�Ԃɏo�肷��
    }

    public void Generate(string genre, int level)
    {
        string question = "��蕶�F";
        string hint = "�q���g�F";
        string answer = "�𓚁F";
        StartCoroutine(_chatGPTClient.SendRequest(level, genre, (q, h, a) => { question += q; hint += h; answer += a; }));

        string temp = "";
        for (int i = 0; i < level; i++)
        {
            temp += "��";
        }
        for (int i = 0; i < 5 - level; i++)    //5�͓�Փx�̍ő�l
        {
            temp += "��";
        }

        _level.text = $"��Փx�F{temp}";
         _question.DOText(question, _textTime);
        _hint.DOText(hint, _textTime);
    }
}
