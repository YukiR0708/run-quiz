using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;



public class ChatGPTClient : MonoBehaviour
{
    [SerializeField] APIData _data = default;
    private readonly string _apiKey = "";　//コンストラクタでAPIを設定する
    private readonly string _apiUrl = "";
    string _question = "";
    string _hint = "";
    string _answer = "";

    /// <summary> APIの設定  </summary>
    private ChatGPTClient()
    {
        _apiKey = _data.APIKey;
        _apiUrl = _data.APIUrl;
    }
    public IEnumerator SendRequest(int level, string genre, System.Action<string, string, string> callback)
    {
        // リクエストの作成
        UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST");

        // リクエストヘッダーの設定
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + _apiKey);

        // リクエストボディの設定
        string jsonRequestBody = "{\"prompt\": \"難易度: " + level + "、ジャンル: " + genre + "の2択クイズの問題文を生成してください。" +
            "なお、難易度は1~5の5段階で選択肢はA,Bとします。ヒントも生成してください。" +
            "出力ではそれぞれ頭に「問題文：」「ヒント：」「解答：」という形でヘッダーをつけてください。\", \"max_tokens\": 50}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // レスポンスの受け取り設定
        request.downloadHandler = new DownloadHandlerBuffer();

        // リクエストの送信
        yield return request.SendWebRequest();

        // レスポンスの処理
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            ParseResponse(response); // レスポンスの解析と結果の格納
            callback(_question, _hint, _answer);
        }
        else
        {
            Debug.LogError("ChatGPT request error: " + request.error);
        }

    }

    //ChatGPTからの出力(JSON)を解析し、「問題」と「解答」と「ヒント」に分ける
    private void ParseResponse(string response)
    {
        // レスポンスの解析と結果の格納
        Dictionary<string, object> jsonData = MiniJSON.Json.Deserialize(response) as Dictionary<string, object>;
        if (jsonData.ContainsKey("choices"))
        {
            List<object> choices = jsonData["choices"] as List<object>;
            if (choices.Count > 0)
            {
                Dictionary<string, object> choiceData = choices[0] as Dictionary<string, object>;
                if (choiceData.ContainsKey("text"))
                {
                    string fullText = choiceData["text"] as string;
                    var extract = Extract(fullText);
                    _question = extract[0];
                    _hint = extract[1];
                    _answer = extract[2];
                }
            }
        }
    }

    private string[] Extract(string fullText)
    {
        string[] extracted = new string[3]; //問題文、ヒント、解答の順で格納する
        // 特定のパターンやキーワードを検索して文を抽出する処理を行う
        // "問題文： ～"という形式のパターンを検索して問題文を抽出する処理。解答とヒントについても同様
        Regex qRegex = new Regex(@"問題文： (.+)");
        Regex hRegex = new Regex(@"ヒント： (.+)");
        Regex aRegex = new Regex(@"解答： (.+)");
        Match[] matches = { qRegex.Match(fullText), hRegex.Match(fullText), aRegex.Match(fullText) };
        for (int i = 0; i < matches.Length; i++)
        {
            extracted[i] = matches[i].Success ? matches[i].Groups[1].Value : "";
        }
        return extracted;
    }

}
