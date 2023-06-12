using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTClient : MonoBehaviour
{
    private const string apiKey = "";�@//���Ƃ�API�ݒ肷��
    private const string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";

    public IEnumerator SendRequest(int level, string genre, System.Action<string> callback)
    {
        // ���N�G�X�g�̍쐬
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // ���N�G�X�g�w�b�_�[�̐ݒ�
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // ���N�G�X�g�{�f�B�̐ݒ�
        string jsonRequestBody = "{\"prompt\": \"��Փx: " + level + "�A�W������: " + genre + "��2���N�C�Y�𐶐����Ă��������B�Ȃ��A��Փx��1~5��5�i�K�ŉ𓚂�A,B�Ƃ��܂��B�q���g���������Ă��������B\", \"max_tokens\": 50}";
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
            callback(response);
        }
        else
        {
            Debug.LogError("ChatGPT request error: " + request.error);
        }
    }
}
