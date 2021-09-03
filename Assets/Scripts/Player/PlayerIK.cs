using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public struct HumanBone
    {
        public HumanBodyBones bone;
    };

    public Transform riflePos;
    [Range(0,1)]
    public float distanceToRifle;
    
    private Animator anim;

    public int iterations = 10;
    public HumanBone[] humanBones;
    public Transform[] boneTransforms;

    public Transform handPosR;
    public Transform handPosL;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
//        boneTransforms = new Transform[humanBones.Length];
        // for (int i = 0; i < boneTransforms.Length; i++)
        // {
        //     boneTransforms[i] = anim.GetBoneTransform(humanBones[i].bone);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            // right hand
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, handPosR.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, handPosR.rotation);

            // left hand
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, handPosL.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, handPosL.rotation);
        }
    }

    private void LateUpdate()
    {

    }
}
