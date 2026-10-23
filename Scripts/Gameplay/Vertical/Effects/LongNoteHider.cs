using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNoteHider : MonoBehaviour
{
    public float speed = 15.0f;
    Coroutine coMove;

    public void ShowHider() // 롱노트 누를 때 마스크 활성화
    {
        if ( coMove != null )
            StopCoroutine("IEHide");
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.85f, gameObject.transform.position.z);
    }


    public void HideHider() // 롱노트 땔 때 마스크 비활성화
    {
        gameObject.SetActive(false);
    }
    
    public void MoveHider() // 롱노트 실패시 롱노트와 함께 밑으로 내려감 ( 잘려보이는 효과 )
    {
        coMove = StartCoroutine("IEHide");
    }

    public IEnumerator IEHide()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.85f, gameObject.transform.position.z);
        while(true)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            if ( transform.position.y < -3.5f )
                gameObject.SetActive(false);
            
            yield return null;
        }
    }
    
}
