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
        Generate("�e�X�g", 3);
    }

    private void Update()
    {
      //UniRX��PlayerValues���Ď���Response�ɂȂ����u�Ԃɏo�肷��
    }

    public void Generate(string genre, int level)
    {
        string question = "��蕶�F";
        StartCoroutine(_chatGPTClient.SendRequest(level, genre, (s) => question += s));

string temp = "";
        for (int i = 0; i < level; i++)
        {
            temp += "��";
        }
        for(int i = 0; i < 5-level; i++)    //5�͓�Փx�̍ő�l
        {
            temp += "��";
        }

        _level.text = $"��Փx�F{temp}";
        //ChatGPT����̏o�͂�ans�ɓ����
        _question.DOText(question, _textTime);
    }
}
