using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class ResultChecker : MonoBehaviour
{
    PlayerValues _pv = default;
    void Start()
    {
        _pv = PlayerValues.Instance;
        //PlayerCondition�̕ύX�ʒm���w��
        _pv.OnBitFlagVariableChanged.Subscribe(newValue =>
        {
            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //���딻�肵�ăX�R�A���Z����
            }

            //PlayerValues�̓V���O���g�������A�O�̂���Dispose�i�w�ǉ����j���Ă���
        }).AddTo(_pv);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
