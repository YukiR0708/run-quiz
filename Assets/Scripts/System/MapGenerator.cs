using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class MapGenerator : SingletonMonoBehaviour<MapGenerator>
{
    [SerializeField, Tooltip("生成するプレハブ")] List<GameObject> Fields = new();
    [SerializeField, Tooltip("一度に生成する数")] int _maxCount = 10;
    [SerializeField, Tooltip("配置の間隔")] float _space = 30f;
    [Tooltip("新規生成する座標")] Vector3 _spawnPos = default;
    [SerializeField, Tooltip("通過したフィールドを削除する基準の座標")] Vector3 _borderPos = default;
    PlayerValues _pv = default;
    ReactiveProperty<int> _passedCount = new ReactiveProperty<int>();
    public IObservable<int> OnPassdeCountChanged => _passedCount;

    public Vector3 BorderPos { get => _borderPos; }
    public float Space { get => _space; }

    protected override bool _dontDestroyOnLoad { get { return true; } }
    void Start()
    {
        _pv = PlayerValues.Instance;
        if (0 < _maxCount)
        {
            List<Vector3> GeneratePos = new();
            for (int i = 0; i < _maxCount; i++)
            {
                //初期に展開する地形の座標を生成
                GeneratePos.Add(new Vector3(0, 0, i * _space));
            }
            //地形を生成
            foreach (var pos in GeneratePos)
            {
                var field = Fields[UnityEngine.Random.Range(0, Fields.Count)];
                Instantiate(field, pos, Quaternion.Euler(0f, 90f, 90f));
            }
            _spawnPos = GeneratePos[GeneratePos.Count - 1];
        }
    }

    /// <summary> Field側からPlayerの通過を検知したら呼ばれるメソッド</summary>
    /// <param name="field"></param>
    public void DestroyAndSpawn(GameObject field)
    {
        var newField = Fields[UnityEngine.Random.Range(0, Fields.Count)];
        Instantiate(newField, _spawnPos, field.transform.rotation);
        Destroy(field);
        if (!_pv.HasFlag(PlayerValues.PlayerCondition.Response)) _passedCount.Value++;
    }

}
