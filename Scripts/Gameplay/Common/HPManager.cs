using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    static HPManager instance;
    public static HPManager Instance { get { return instance; } }

    private UISlider HPSlider = null;

    private int hp = 100; // 비트 쪼개기용 ( 1 - 64비트 2 - 32비트 ... )
    public int HP
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, 100);
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        HP = 100;
        HPSlider = UIController.Instance.GetUI("UI_HPGage").uiObject as UISlider;
    }

    public void AdjustHP(int value)
    {
        if (HPSlider != null)
            HP += value;
            HPSlider.slider.value = HP;

        if (HP < 0)
            HpGameOver();
    }

    public void HpGameOver() // 임시 게임 오버
    {
        MusicPlayer.Instance.Stop();
        NoteGenerator.Instance.StopGen();
    }

    private void OnDestroy() {
        instance = null;
    }
}
