using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;



public class ChatGPTClient : MonoBehaviour
{
    [SerializeField] APIData _data = default;
    private readonly string _apiKey = "";�@//�R���X�g���N�^��API��ݒ肷��
    private readonly string _apiUrl = "";
    string _question = "";
    string _hint = "";
    string _answer = "";

    /// <summary> API�̐ݒ�  </summary>
    private ChatGPTClient()
    {
        _apiKey = _data.APIKey;
        _apiUrl = _data.APIUrl;
    }
    public IEnumerator SendRequest(int level, string genre, System.Action<string, string, string> callback)
    {
        // ���N�G�X�g�̍쐬
        UnityWebRequest request = new UnityWebRequest(_apiUrl, "POST");

        // ���N�G�X�g�w�b�_�[�̐ݒ�
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + _apiKey);

        // ���N�G�X�g�{�f�B�̐ݒ�
        string jsonRequestBody = "{\"prompt\": \"��Փx: " + level + "�A�W������: " + genre + "��2���N�C�Y�̖�蕶�𐶐����Ă��������B" +
            "�Ȃ��A��Փx��1~5��5�i�K�őI������A,B�Ƃ��܂��B�q���g���������Ă��������B" +
            "�o�͂ł͂��ꂼ�ꓪ�Ɂu��蕶�F�v�u�q���g�F�v�u�𓚁F�v�Ƃ����`�Ńw�b�_�[�����Ă��������B\", \"max_tokens\": 50}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // ���X�|���X�̎󂯎��ݒ�
        request.downloadHandler = new DownloadHandlerBuffer();

        // ���N�G�X�g�̑��M
        yield return request.SendWebRequest();

        // ���X�|���X�̏���
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            ParseResponse(response); // ���X�|���X�̉�͂ƌ��ʂ̊i�[
            callback(_question, _hint, _answer);
        }
        else
        {
            Debug.LogError("ChatGPT request error: " + request.error);
        }

    }

    //ChatGPT����̏o��(JSON)����͂��A�u���v�Ɓu�𓚁v�Ɓu�q���g�v�ɕ�����
    private void ParseResponse(string response)
    {
        // ���X�|���X�̉�͂ƌ��ʂ̊i�[
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
        string[] extracted = new string[3]; //��蕶�A�q���g�A�𓚂̏��Ŋi�[����
        // ����̃p�^�[����L�[���[�h���������ĕ��𒊏o���鏈�����s��
        // "��蕶�F �`"�Ƃ����`���̃p�^�[�����������Ė�蕶�𒊏o���鏈���B�𓚂ƃq���g�ɂ��Ă����l
        Regex qRegex = new Regex(@"��蕶�F (.+)");
        Regex hRegex = new Regex(@"�q���g�F (.+)");
        Regex aRegex = new Regex(@"�𓚁F (.+)");
        Match[] matches = { qRegex.Match(fullText), hRegex.Match(fullText), aRegex.Match(fullText) };
        for (int i = 0; i < matches.Length; i++)
        {
            extracted[i] = matches[i].Success ? matches[i].Groups[1].Value : "";
        }
        return extracted;
    }

}
