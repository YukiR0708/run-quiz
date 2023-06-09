using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookIK : MonoBehaviour
{
    [SerializeField] Transform _lookTarget = default;
    [SerializeField, Range(0f, 1f)] float _lookWeight = 0f;
    Animator _anim = default;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        _anim.SetLookAtWeight(_lookWeight);
        _anim.SetLookAtPosition(_lookTarget.position);
    }
}
