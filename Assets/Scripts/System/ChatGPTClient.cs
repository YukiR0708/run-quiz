using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTClient : MonoBehaviour
{
    private const string apiKey = "";　//あとでAPI設定する
    private const string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    public IEnumerator SendRequest(int level, string genre, System.Action<string> callback)
    {
        // リクエストの作成
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // リクエストヘッダーの設定
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // リクエストボディの設定
        string jsonRequestBody = "{\"prompt\": \"難易度: " + level + "、ジャンル: " + genre + "の2択クイズを生成してください。なお、難易度は1~5の5段階で解答はA,Bとします。ヒントも生成してください。\", \"max_tokens\": 50}";
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
            callback(response);
        }
        else
        {
            Debug.LogError("ChatGPT request error: " + request.error);
        }
    }
}
