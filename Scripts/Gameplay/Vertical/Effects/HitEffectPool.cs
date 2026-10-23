using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HitEffectPool : MonoBehaviour
{
    static HitEffectPool instance;
    public static HitEffectPool Instance { get { return instance; } }

    public GameObject hitPrefab;
    public GameObject parent;
    public Transform[] linePosProvider;
    public float[] linePos = { -6.47f, -4.812f, -3.163f, -1.522f };

    void Awake()
    {
        if (instance == null)
            instance = this;
        for (int i = 0; i < 4; i++) linePos[i] = linePosProvider[i].position.x;
    }

    IObjectPool<GameObject> poolHitEffect;
    public IObjectPool<GameObject> PoolHitEffect
    {
        get
        {
            if (poolHitEffect == null)
                poolHitEffect = new ObjectPool<GameObject>(CreateHitEffect, defaultCapacity: 32);
            return poolHitEffect;
        }
    }

    GameObject CreateHitEffect()
    {
        GameObject hitEffect = Instantiate(hitPrefab, parent.transform);
        return hitEffect;
    }



    public void ShowHitEffect(int i) // Pool을 이용하여 해당 라인에 노트 효과 생성
    {
        GameObject effect = PoolHitEffect.Get();
        effect.transform.position = new Vector3(linePosProvider[i].position.x, 0, 0); // -0.14f
    }

    public void SetHitEffect(GameObject newHitEffect)
    {
        hitPrefab = newHitEffect;
    }
}
