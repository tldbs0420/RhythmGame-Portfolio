using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DanceHitEffectPool : MonoBehaviour
{
    static DanceHitEffectPool instance;
    public static DanceHitEffectPool Instance { get { return instance; } }

    void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    public GameObject hitPrefab;
    public GameObject parent;
    readonly float[] linePosX = { -2.5f, -2.5f, 2.5f, 2.5f };
    readonly float[] linePosY = { 2.5f, 1f, -1f, -2.5f };



    IObjectPool<GameObject> poolHitEffect;
    public IObjectPool<GameObject> PoolHitEffect
    {
        get
        {
            if ( poolHitEffect == null )
                poolHitEffect = new ObjectPool<GameObject>(CreateHitEffect, defaultCapacity: 32);
            return poolHitEffect;
        }
    }

    GameObject CreateHitEffect()
    {
        GameObject hitEffect = Instantiate(hitPrefab, parent.transform);
        return hitEffect;
    }



    public void ShowHitEffect( int i ) // Pool을 이용하여 해당 라인에 노트 효과 생성
    {
        GameObject effect = PoolHitEffect.Get();
        effect.transform.position = new Vector3(linePosX[i], linePosY[i] , 0);
    }
}
