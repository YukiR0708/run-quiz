using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum GameMode
    {
        Title,
        InGame,
        Pause,
        InPause,
        Result,
        None ,
    }
    [SerializeField]GameMode _nowMode = GameMode.None;
    public GameMode NowMode { get => _nowMode; set =>_nowMode = value; }

    protected override bool _dontDestroyOnLoad { get { return true; } }

    void Start()
    {

    }

    void Update()
    {

    }
}
