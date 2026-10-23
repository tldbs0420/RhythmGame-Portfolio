using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private void OnDestroy() { // Destroy 전 Pool로 자기자신 반납
        HitEffectPool.Instance.PoolHitEffect.Release(gameObject as GameObject);
    }
}
