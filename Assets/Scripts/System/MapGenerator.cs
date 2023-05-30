using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : SingletonMonoBehaviour<MapGenerator>
{
    [SerializeField, Tooltip("��������v���n�u")] List<GameObject> Fields = new();
    [SerializeField, Tooltip("�t�B�[���h�𐶐�����|�W�V����")]List<Vector3> GeneratePos = new();
    [SerializeField, Tooltip("��x�ɐ������鐔")] int _maxCount = 0;
    [SerializeField, Tooltip("�z�u�̊Ԋu")] float _offset = 0f;
    public float Offset{ get => _offset;}
    [SerializeField] PlayerJump _player = default;

    protected override bool _dontDestroyOnLoad { get { return true; } }
    void Start()
    {
        for(int i = 0; i < _maxCount; i++)
        {
            //�����ɓW�J����n�`�̍��W�𐶐�
            GeneratePos.Add(new Vector3(0, 0, i * _offset));
        }
        //�n�`�𐶐�
        foreach(var pos in GeneratePos)
        {
            var field = Fields[Random.Range(0, Fields.Count - 1)];
            Instantiate(field, pos, Quaternion.identity);
        }
    }

/// <summary> Field������Player�̒ʉ߂����m������Ă΂�郁�\�b�h</summary>
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
