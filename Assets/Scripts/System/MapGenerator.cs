using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : SingletonMonoBehaviour<MapGenerator>
{
    [SerializeField, Tooltip("生成するプレハブ")] List<GameObject> Fields = new();
    [SerializeField, Tooltip("フィールドを生成するポジション")]List<Vector3> GeneratePos = new();
    [SerializeField, Tooltip("一度に生成する数")] int _maxCount = 0;
    [SerializeField, Tooltip("配置の間隔")] float _offset = 0f;
    public float Offset{ get => _offset;}
    [SerializeField] PlayerJump _player = default;

    protected override bool _dontDestroyOnLoad { get { return true; } }
    void Start()
    {
        for(int i = 0; i < _maxCount; i++)
        {
            //初期に展開する地形の座標を生成
            GeneratePos.Add(new Vector3(0, 0, i * _offset));
        }
        //地形を生成
        foreach(var pos in GeneratePos)
        {
            var field = Fields[Random.Range(0, Fields.Count - 1)];
            Instantiate(field, pos, Quaternion.identity);
        }
    }

/// <summary> Field側からPlayerの通過を検知したら呼ばれるメソッド</summary>
/// <param name="field"></param>
   public  void DestroyAndSpawn(GameObject field)
    {
        GeneratePos.RemoveAt(0);
        Destroy(field);
        var nextPos = new Vector3(0, 0, GeneratePos[GeneratePos.Count - 1].z + _offset);
        GeneratePos.Add(nextPos);
        var newField = Fields[Random.Range(0, Fields.Count - 1)];
        Instantiate(newField, GeneratePos[GeneratePos.Count - 1], Quaternion.identity);
    }

}
