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
        //PlayerConditionの変更通知を購読
        _pv.OnBitFlagVariableChanged.Subscribe(newValue =>
        {
            if (newValue.HasFlag(PlayerValues.PlayerCondition.AnswerCheck))
            {
                //正誤判定してスコア加算する
            }

            //PlayerValuesはシングルトンだが、念のためDispose（購読解除）しておく
        }).AddTo(_pv);
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
