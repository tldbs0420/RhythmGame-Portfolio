using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHitLine : MonoBehaviour
{
    Coroutine line1;
    Coroutine line2;
    Coroutine line3;
    Coroutine line4;

    public float offset = 1f;
    private int combo = 0;

    [SerializeField] Transform[] effectPositions;
    [SerializeField] GameAnimations gameAnimations;
    [SerializeField] UIText uiJudgement;
    [SerializeField] UIText uiCombo;

    void Start()
    {
        gameAnimations.Join(uiJudgement.Name);
        gameAnimations.Join(uiCombo.Name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.position.x);
        Debug.Log(effectPositions[0].position.x);
        if (other.transform.position.x <= effectPositions[0].position.x + offset)
            line1 = StartCoroutine(StartEffect(0));
        else if (other.transform.position.x <= effectPositions[1].position.x + offset)
            line2 = StartCoroutine(StartEffect(1));
        else if (other.transform.position.x <= effectPositions[2].position.x + offset)
            line3 = StartCoroutine(StartEffect(2));
        else if (other.transform.position.x <= effectPositions[3].position.x + offset)
            line4 = StartCoroutine(StartEffect(3));
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.position.x <= effectPositions[0].position.x + offset)
            StopCoroutine(line1);
        else if (other.transform.position.x <= effectPositions[1].position.x + offset)
            StopCoroutine(line2);
        else if (other.transform.position.x <= effectPositions[2].position.x + offset)
            StopCoroutine(line3);
        else if (other.transform.position.x <= effectPositions[3].position.x + offset)
            StopCoroutine(line4);
    }

    IEnumerator StartEffect(int n)
    {
        while (true)
        {
            GameObject gameObject = GameManager.Instance.GetPlayerCustomize().hitEffectData.hitEffect;

            Vector3 newPosition = effectPositions[n].position;
            gameObject = Instantiate(gameObject, newPosition, Quaternion.identity);
            gameObject.transform.parent = this.transform;

            combo++;
            uiCombo.SetText(combo.ToString());

            uiJudgement.SetColor(GameManager.Instance.GetPlayerCustomize().gearData.personalColor);
            uiJudgement.SetText(GameManager.Instance.GetPlayerCustomize().gearData.perfect);
            gameAnimations.PlayPop(uiCombo.Name, uiCombo.rect);
            gameAnimations.PlayBig(uiJudgement.Name, uiJudgement.rect);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
