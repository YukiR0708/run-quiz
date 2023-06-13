using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIData : ScriptableObject
{
    [SerializeField] string _apiKey = "";
    [SerializeField] string _apiUrl = "";

    public string APIKey{ get => _apiKey; }
    public string APIUrl { get => _apiUrl;}
}
