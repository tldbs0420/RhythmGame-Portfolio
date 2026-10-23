using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehavior : StateMachineBehaviour // 노트 이펙드 생성시 스스로 Destroy
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
