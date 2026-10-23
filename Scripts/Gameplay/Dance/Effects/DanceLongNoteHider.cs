using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceLongNoteHider : MonoBehaviour
{
    public float speed = 9.0f;
    public int moveDirection = 1;
    Coroutine coMove;

    public void ShowHider()
    {
        if ( coMove != null )
            StopCoroutine("IEHide");
        gameObject.SetActive(true);
        if ( moveDirection == 1 )
            gameObject.transform.position = new Vector3(-0.58f, gameObject.transform.position.y, gameObject.transform.position.z);
        else if ( moveDirection == -1 )
            gameObject.transform.position = new Vector3(0.58f, gameObject.transform.position.y, gameObject.transform.position.z);
    }


    public void HideHider()
    {
        gameObject.SetActive(false);
    }
    
    public void MoveHider()
    {
        coMove = StartCoroutine("IEHide");
    }

    public IEnumerator IEHide()
    {
        if ( moveDirection == 1 )
            gameObject.transform.position = new Vector3(-0.58f, gameObject.transform.position.y, gameObject.transform.position.z);
        else if ( moveDirection == -1 )
            gameObject.transform.position = new Vector3(0.58f, gameObject.transform.position.y, gameObject.transform.position.z);
        while(true)
        {
            transform.position += Vector3.right * speed * Time.deltaTime * moveDirection;
            if ( ( transform.position.x > 2.25 && moveDirection == 1 ) || ( transform.position.x < -2.25 && moveDirection == -1 ) )
                gameObject.SetActive(false);
            
            yield return null;
        }
    }
    
}
