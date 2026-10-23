using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class Category : MonoBehaviour
{
    public RectTransform contentPanelTransform;
    public GameObject nothing;
    public GameObject list;

    static Category instance;
    public static Category Instance
    {
        get
        {
            return instance;
        }
    }

    public TextMeshProUGUI[] categories;
    private string[] names = new string[] {
        "즐겨찾기", "전체", "스텔라이브", "아야츠노 유니", "아이리 칸나", "아라하시 타비", "네네코 마시로", "시라유키 히나", "아카네 리제",
        "텐코 시부키", "아오쿠모 린", "하나코 나나", "유즈하 리코", "강지"
    };
    public Color[] colors = new Color[] {
        new Color(1, 1, 0), new Color(1, 1, 1), new Color(0.5176471f, 0.627451f, 0.9019608f), new Color(0.5254902f, 0.5137255f, 0.8666667f), new Color(0.09019608f, 0.2392157f, 0.4941176f),
        new Color(0.6039216f, 0.854902f, 1), new Color(0.6745098f, 0.6431373f, 0.6352941f), new Color(0.9647059f, 0.7137255f, 0.7882353f), new Color(0.6235294f, 0.3019608f, 0.3019608f),
        new Color(0.8470588f, 0.7843137f, 0.9490196f), new Color(0.003921569f, 0.2196078f, 0.9215686f), new Color(0.8862745f, 0.509804f, 0.5450981f), new Color(0.7019608f, 0.8823529f, 0.7490196f),
        new Color(0.3686275f, 0.4509804f, 0.5333334f)
    };

    string select = "아라하시 타비";
    int highest;
    int lowest;
    public int sel;
    int selectIndex;

    public Image backgroundImage;  // 배경색을 채울 Image
    public Image backgroundImage2;  // 배경색을 채울 Image
    public float animationDuration = 1.0f;  // 애니메이션 지속 시간

    private Color targetColor;

    Coroutine rightAnimation;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        select = PlayerPrefs.GetString("LastCategory");
        highest = 0;
        lowest = 8;
        sel = 1;
        selectIndex = Array.IndexOf(names, select);
        int finalIndex = 0;
        for (int i = 0; i < 9; i++)
        {
            finalIndex = (selectIndex + i - 1) % names.Length;
            while (finalIndex < 0)
                finalIndex += names.Length;
            categories[i].SetText(names[finalIndex]);
            categories[i].color = colors[finalIndex];
        }

        contentPanelTransform.localPosition = new Vector3(-240,
            contentPanelTransform.localPosition.x,
            contentPanelTransform.localPosition.z);

        categories[sel].color = Color.black;
        backgroundImage.color = colors[selectIndex];
        backgroundImage2.color = colors[selectIndex];
    }

    public void Next()
    {
        categories[highest].gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        //Debug.Log("button" + highest + "moved down");

        selectIndex += 1;
        if (selectIndex > names.Length -1)
            selectIndex = 0;
        sel += 1;
        if (sel > 8)
            sel = 0;
        //Debug.Log("selectIndex = " + selectIndex);

        int finalIndex = (selectIndex + 7) % names.Length;
        CheckFavorite();
        //Debug.Log("finalIndex = " + finalIndex);
        categories[highest].SetText(names[finalIndex]);
        categories[highest].color = colors[finalIndex];
        categories[sel].color = Color.black;
        ChangeColor(colors[selectIndex], 0);
        //Debug.Log("song : " + songs[finalIndex]);

        highest += 1;
        if (highest > 8)
            highest = 0;
        //Debug.Log("highest = " + highest);

        lowest += 1;
        if (lowest > 8)
            lowest = 0;
        //Debug.Log("lowest = " + lowest);
    }

    public void Prev()
    {
        categories[lowest].gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
        // Debug.Log("button" + highest + "moved up");

        //buttons[highest].gameObject.GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(0, height * 8, 0);
        selectIndex -= 1;
        if (selectIndex < 0)
            selectIndex = names.Length -1;
        sel -= 1;
        if (sel < 0)
            sel = 8;
        // Debug.Log("selectIndex = " + selectIndex);

        int finalIndex = (selectIndex - 1) % names.Length;
        while (finalIndex < 0)
                finalIndex += names.Length;
        CheckFavorite();
        // Debug.Log("finalIndex = " + finalIndex);
        categories[lowest].SetText(names[finalIndex]);
        categories[lowest].color = colors[finalIndex];
        categories[sel].color = Color.black;
        categories[(sel + 1) % 9].color = colors[(selectIndex + 1) % names.Length];
        ChangeColor(colors[selectIndex], 1);
        // Debug.Log("song : " + songs[finalIndex]);

        highest -= 1;
        if (highest < 0)
            highest = 8;
        //Debug.Log("highest = " + highest);

        lowest -= 1;
        if (lowest < 0)
            lowest = 8;
        // Debug.Log("lowest = " + lowest);
    }

    public void ChangeColor(Color newColor, int i)
    {
        if (rightAnimation == null)
        {
            targetColor = newColor;  // 변경할 색상 설정
            rightAnimation=StartCoroutine(FillColorCoroutine(i));
        }
        else
        {
            StopCoroutine(rightAnimation);
            backgroundImage.fillAmount = 1.0f;  // 애니메이션 끝나면 최종 Fill Amount
            backgroundImage.color = targetColor;  // 최종 색상 적용
            backgroundImage2.color = targetColor;
            contentPanelTransform.localPosition = new Vector3(-240, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);  // 최종 위치 설정
            targetColor = newColor;  // 변경할 색상 설정
            rightAnimation=StartCoroutine(FillColorCoroutine(i));
        }
    }

    private IEnumerator FillColorCoroutine(int i)
    {
        float elapsedTime = 0f;
        backgroundImage.fillAmount = 0;
        backgroundImage2.color = backgroundImage.color;

        if (i == 0)
            backgroundImage.fillOrigin = (int)Image.OriginHorizontal.Right;
        else
            backgroundImage.fillOrigin = (int)(Image.OriginHorizontal.Left);

        // 텍스트 이동
        if (i == 0)
            contentPanelTransform.localPosition = new Vector3(0,
                contentPanelTransform.localPosition.y,
                contentPanelTransform.localPosition.z);
        else
            contentPanelTransform.localPosition = new Vector3(-480,
                contentPanelTransform.localPosition.y,
                contentPanelTransform.localPosition.z);
        Vector3 initialPosition = contentPanelTransform.localPosition;  // 초기 위치 저장
        Vector3 finalPosition = new Vector3(-240, initialPosition.y, initialPosition.z);  // 이동할 최종 위치 (예제)

        backgroundImage.color = targetColor;

        // 빠르게 채워지기와 느리게 채워지기 분리
        while (elapsedTime < 0.5f)
        {
            float test = elapsedTime / 0.5f;
            float easeX = Mathf.Sqrt(1 - Mathf.Pow(test - 1, 2));
            // Debug.Log("Time : " + elapsedTime);
            // Debug.Log(easeX);

            // float fastT = Mathf.Clamp01(elapsedTime / fastDuration);
            // float slowT = Mathf.Clamp01((elapsedTime - fastDuration) / (animationDuration - fastDuration));

            // float finalFillAmount;
            // if (elapsedTime <= fastDuration)
            // {
            //     finalFillAmount = Mathf.Lerp(0, fastFillAmount, fastT);
            // }
            // else
            // {
            //     finalFillAmount = Mathf.Lerp(fastFillAmount, fastFillAmount + slowFillAmount, slowT);
            // }

            backgroundImage.fillAmount = easeX;

            // 배경색 채우기
            // backgroundImage.fillAmount = finalFillAmount;  // Fill Amount 조정

            // 텍스트 이동
            if (elapsedTime <= 0.2f)  // 이동 시간 조정
            {
                float moveT = Mathf.Clamp01(elapsedTime / 0.2f);
                contentPanelTransform.localPosition = Vector3.Lerp(initialPosition, finalPosition, moveT);
            }
            else{
                contentPanelTransform.localPosition = finalPosition;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        backgroundImage.fillAmount = 1.0f;  // 애니메이션 끝나면 최종 Fill Amount
        backgroundImage.color = targetColor;  // 최종 색상 적용
        backgroundImage2.color = targetColor;
        contentPanelTransform.localPosition = finalPosition;  // 최종 위치 설정
    }

    private void CheckFavorite()
    {
        if (selectIndex == 0 && MusicListManager.Instance.isListEmpty(0))
        {
            nothing.SetActive(true);
            list.SetActive(false);
            MusicScroll.Instance.SetScrollable(false);
        }
        else
        {
            nothing.SetActive(false);
            list.SetActive(true);
            MusicScroll.Instance.SetScrollable(true);
            MusicScroll.Instance.UpdateList();
        }
    }

    public int GetCategoryIndex(string name)
    {
        name = name?.Trim() ?? string.Empty;

        if (name.Contains(",") || name.Contains("스텔라이브") || name.Contains("클리셰") || name.Contains("유니버스"))
            return 2; // 스텔라이브
            
        int index = Array.IndexOf(names, name);
        // 등록되지 않은 이름은 전체 카테고리 색상 사용
        return index >= 0 ? index : 1;
    }

    public string GetCategoryName()
    {
        return categories[sel].text;
    }
}
