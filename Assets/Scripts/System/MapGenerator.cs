using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.ProBuilder;
using static PlayerValues;

public class MapGenerator : SingletonMonoBehaviour<MapGenerator>
{
    [SerializeField, Tooltip("��������v���n�u")] List<GameObject> Fields = new();
    [SerializeField, Tooltip("��x�ɐ������鐔")] int _maxCount = 10;
    [SerializeField, Tooltip("�z�u�̊Ԋu")] float _space = 30f;
    [Tooltip("�V�K����������W")] Vector3 _spawnPos = default;
    [SerializeField, Tooltip("�ʉ߂����t�B�[���h���폜�����̍��W")] Vector3 _borderPos = default;
    PlayerValues _pv = default;
    ReactiveProperty<int> _passedCount = new ReactiveProperty<int>();
    public IObservable<int> OnPassdeCountChanged => _passedCount;

    public Vector3 BorderPos => _borderPos; 

    protected override bool _dontDestroyOnLoad { get { return true; } }
    void Start()
    {
        ReStart();
    }

    /// <summary>�{�[�_�[��ʉ߂����玩�M�������Ď���Predab�𐶐�����BField������Player�̒ʉ߂����m������Ă΂�郁�\�b�h</summary>
    /// <param name="field"></param>
    public void DestroyAndSpawn(GameObject field)
    {
        //��������t�B�[���h�����肷��
        var newField = Fields[UnityEngine.Random.Range(0, Fields.Count)];
        Instantiate(newField, _spawnPos, field.transform.rotation);
        Destroy(field);
        if (!_pv.HasFlag(PlayerValues.PlayerCondition.Response))
        {
            _passedCount.Value++;
        }
    }

    public void ChangeMaterials(Material m1, Material m2)
    {
        GameObject[] fields = GameObject.FindGameObjectsWithTag("Field");
        foreach(var field in fields)
        {
            Material[] tmp = field.GetComponent<Renderer>().materials;
            tmp[0] = m1;
            tmp[1] = m2;
            field.GetComponent<Renderer>().materials = tmp;
        }

        
    }
    public override void ReStart()
    {
        _pv = PlayerValues.Instance;
        var fields = GameObject.FindGameObjectsWithTag("Field");
        if(fields.Length > 0)
        {
            foreach(var field in fields)Destroy(field.gameObject);
        }
        if (0 < _maxCount)
        {
            List<Vector3> GeneratePos = new();
            for (int i = 0; i < _maxCount; i++)
            {
                //�����ɓW�J����n�`�̍��W�𐶐�
                GeneratePos.Add(new Vector3(0, 0, i * _space));
            }
            //�n�`�𐶐�
            foreach (var pos in GeneratePos)
            {
                var field = Fields[UnityEngine.Random.Range(0, Fields.Count)];
                Instantiate(field, pos, Quaternion.Euler(0f, 90f, 90f));
            }
            _spawnPos = GeneratePos[GeneratePos.Count - 1];
        }
        _passedCount.Dispose();
        _passedCount = new ReactiveProperty<int>();
        _passedCount.Value = 0;
    }
}
