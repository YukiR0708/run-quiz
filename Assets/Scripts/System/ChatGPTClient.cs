using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenAI_API;
using System.Threading.Tasks;


public class ChatGPTClient : MonoBehaviour
{
    [SerializeField] APIData _data = default;
    string[] extracts = new string[5];
    OpenAIAPI _openAI = default;

    private void Awake()
    {
        _openAI = new OpenAIAPI(_data.APIKey);
    }



    public async Task SendRequestAsync(int level, string genre, System.Action<string[]> callback)
    {
        var chat = _openAI.Chat.CreateConversation();

        chat.AppendUserInput($"�ȉ��̏�����2���̐�����𐶐����Ă��������B��Փx��5�i�K��{level}�A�W��������{genre}�ŁA" +
            $"�܂��A�q���g���������A�u��蕶�F�v�u�I����A�F�v�u�I����B�F�u�q���g�F�v�u�𓚁F�v(�i�𓚂�A��B�ŕԓ��j�Ƃ����`�ŏo�͂��Ă��������B" +
            $"�q���g�͊ȒP�����Ȃ��悤�ɂ��Ă��������B" +
            $"" +
            $"");

        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
       await  ParseResponseAsync(response); // ���X�|���X�̉�͂ƌ��ʂ̊i�[
        callback(extracts);
    }

    // ChatGPT����̏o�͂�񓯊��ɉ�͂��A�u���v�Ɓu�𓚁v�Ɓu�q���g�v�ɕ�����
    private async Task ParseResponseAsync(string response)
    {
        Regex[] regexPatterns =
        {
        new Regex(@"��蕶�F(.+)"),
        new Regex(@"�I����A�F(.+)"),
        new Regex(@"�I����B�F(.+)"),
        new Regex(@"�q���g�F(.+)"),
        new Regex(@"�𓚁F(.+)")
    };

        for (int i = 0; i < regexPatterns.Length; i++)
        {
            extracts[i] = ExtractMatchText(regexPatterns[i], response);
            Debug.Log(extracts[i]);
        }
    }

    // ���K�\���p�^�[���Ɉ�v����e�L�X�g�𒊏o����
    private string ExtractMatchText(Regex regex, string input)
    {
        Match match = regex.Match(input);
        if (match.Success)
        {
            Debug.Log("Match found: " + match.Value);
            return match.Groups[1].Value.Trim();
        }
        else
        {
            Debug.Log("No match found for pattern: " + regex.ToString());
            return string.Empty;
        }
    }
}
