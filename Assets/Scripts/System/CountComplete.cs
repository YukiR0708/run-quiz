using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class CountComplete : MonoBehaviour
{
    GameManager _gm = default;

    private void Start()
    {
        _gm = GameManager.Instance;
    }
    public void Completed()
    {
        _gm.NowMode = GameMode.InGame;
        this.gameObject.SetActive(false);
    }
}
