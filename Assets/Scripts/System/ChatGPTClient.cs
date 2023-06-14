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

        chat.AppendUserInput($"以下の条件で2択の正誤問題を生成してください。難易度は5段階中{level}、ジャンルは{genre}で、" +
            $"また、ヒントも生成し、「問題文：」「選択肢A：」「選択肢B：「ヒント：」「解答：」(（解答はAかBで返答）という形で出力してください。" +
            $"ヒントは簡単すぎないようにしてください。" +
            $"" +
            $"");

        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
       await  ParseResponseAsync(response); // レスポンスの解析と結果の格納
        callback(extracts);
    }

    // ChatGPTからの出力を非同期に解析し、「問題」と「解答」と「ヒント」に分ける
    private async Task ParseResponseAsync(string response)
    {
        Regex[] regexPatterns =
        {
        new Regex(@"問題文：(.+)"),
        new Regex(@"選択肢A：(.+)"),
        new Regex(@"選択肢B：(.+)"),
        new Regex(@"ヒント：(.+)"),
        new Regex(@"解答：(.+)")
    };

        for (int i = 0; i < regexPatterns.Length; i++)
        {
            extracts[i] = ExtractMatchText(regexPatterns[i], response);
            Debug.Log(extracts[i]);
        }
    }

    // 正規表現パターンに一致するテキストを抽出する
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
