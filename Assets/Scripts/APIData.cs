using UnityEngine;

public class APIData : ScriptableObject
{
    [SerializeField] string _apiKey = "";

    public string APIKey => _apiKey;
}
