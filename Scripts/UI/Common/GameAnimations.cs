using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimations : MonoBehaviour
{
    static GameAnimations instance;
    public static GameAnimations Instance { get { return instance; } }

    private Dictionary<string, bool> signalDic = new Dictionary<string, bool>();
    private float yDiff = 0f; // 콤보 애니메이션 재생에 필요한 시작 y좌표와 끝 y좌표의 차이 값을 담을 변수
    private int speed = 12; // 재생 속도
    private float duration = 1f; // 재생 시간


    public void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    public void Join(string uiName)
    {
        signalDic.Add(uiName, true);
    }


    public void PlayPop(string uiName, RectTransform rect)
    {
        if (signalDic[uiName])
            StartCoroutine(IEAniPop(uiName, rect));
    }

    public IEnumerator IEAniPop(string uiName, RectTransform rect) // 콤보 애니메이션 (아래서 위로 등장)
    {
        signalDic[uiName] = false;
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition3D.x, rect.anchoredPosition3D.y - yDiff, rect.anchoredPosition3D.z );
        Vector3 originPos = rect.anchoredPosition3D;

        float time = 0f;
        while ( time < duration )
        {
            rect.anchoredPosition3D = new Vector3(originPos.x, originPos.y + 50f * time, originPos.z );

            time += Time.deltaTime * speed;
            yield return null;
        }
        yDiff = rect.anchoredPosition3D.y - originPos.y;
        signalDic[uiName] = true;
    }


    public void PlayBig(string uiName, RectTransform rect)
    {
        if ( signalDic[uiName] )
            StartCoroutine(IEAniBig(uiName, rect));
    }

    public IEnumerator IEAniBig(string uiName, RectTransform rect) // 판정 애니메이션 (커지며 등장)
    {
        signalDic[uiName] = false;
        Vector3 originPos = rect.anchoredPosition3D;

        float time = 0f;
        while ( time < duration )
        {
            rect.localScale = Vector3.one * (time + 0.05f);
            time += Time.deltaTime * speed;
            yield return null;
        }

        rect.anchoredPosition3D = originPos;
        signalDic[uiName] = true;
    }
    private void OnDestroy() {
        instance = null;
    }
}
