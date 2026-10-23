using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Preview : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] GameObject notes;
    [SerializeField] SpriteRenderer gear;
    [SerializeField] SpriteRenderer hitLine;
    [SerializeField] GameObject[] noteObjects;
    [SerializeField] GameObject character;
    [SerializeField] Image timer;
    [SerializeField] SpriteRenderer hpBar;

    void Update()
    {
        gear.sprite = GameManager.Instance.GetPlayerCustomize().gearData.gear;
        hitLine.sprite = GameManager.Instance.GetPlayerCustomize().hitLineData.hitLine;
        for (int i = 0; i < 12; i++)
        {
            if (i % 4 == 0)
                noteObjects[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetPlayerCustomize().line1.note;
            else if (i % 4 == 1)
                noteObjects[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetPlayerCustomize().line2.note;
            else if (i % 4 == 2)
                noteObjects[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetPlayerCustomize().line3.note;
            else if (i % 4 == 3)
                noteObjects[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetPlayerCustomize().line4.note;
        }
        noteObjects[4].GetComponent<LineRenderer>().material.color = GameManager.Instance.GetPlayerCustomize().line1.lineColor;
        noteObjects[5].GetComponent<LineRenderer>().material.color = GameManager.Instance.GetPlayerCustomize().line2.lineColor;
        noteObjects[6].GetComponent<LineRenderer>().material.color = GameManager.Instance.GetPlayerCustomize().line3.lineColor;
        noteObjects[7].GetComponent<LineRenderer>().material.color = GameManager.Instance.GetPlayerCustomize().line4.lineColor;
        timer.sprite = GameManager.Instance.GetPlayerCustomize().gearData.timer;
        hpBar.color = GameManager.Instance.GetPlayerCustomize().gearData.hpColor;


        notes.transform.position = notes.transform.position + Vector3.down * speed * Time.deltaTime;
        if (notes.transform.position.y < -35f) notes.transform.position = new Vector3(notes.transform.position.x, notes.transform.position.y + 45f, notes.transform.position.z);
    }

}
