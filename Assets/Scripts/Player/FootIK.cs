using UnityEngine;

public class FootIK : MonoBehaviour
{
    [SerializeField] Transform _rightTarget = default;
    [SerializeField] Transform _leftTarget = default;
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0f;
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0f;
    [SerializeField, Range(0f, 1f)] float _rightRotationWeight = 0f;
    [SerializeField, Range(0f, 1f)] float _leftRotationWeight = 0f;
    Animator _anim = default;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //âEë´Ç…IKÇê›íËÇ∑ÇÈ
        _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, _rightPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, _rightRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightFoot, _rightTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightFoot, _rightTarget.rotation);
        //ç∂ë´Ç…IKÇê›íËÇ∑ÇÈ
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _leftPositionWeight);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _leftRotationWeight);
        _anim.SetIKPosition(AvatarIKGoal.LeftFoot, _leftTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftFoot, _leftTarget.rotation);
    }
}
