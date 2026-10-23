using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    Canvas canvas;
    public Button btn;
    Image img;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public bool isInteractable = false;

    void Start()
    {
        isInteractable = false;
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        btn = GetComponent<Button>();
        btn.interactable = false;
        img = GetComponent<Image>();
        animator = GetComponent<Animator>();
        canvas = transform.parent.GetComponent<Canvas>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerEnter()
    {
        if (isInteractable)
        {
            canvas.sortingOrder = 12;
            spriteRenderer.sortingOrder = 13;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            btn.Select();
        }
    }
    
    public void OnPointerExit()
    {
        if (isInteractable)
        {
            canvas.sortingOrder = 10;
            spriteRenderer.sortingOrder = 11;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.9f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.9f);
        }
    }

    public void OnClick()
    {
        canvas.sortingOrder = 14;
        spriteRenderer.sortingOrder = 15;
        animator.SetBool("Complete", true);
        animator.Play("Pressed");
        if (this.gameObject.name == "Play" && isInteractable == true)
        {
            isInteractable = false;
            btn.interactable = false;
            GameManager.Instance.PlaySelect();
        }
        else if (this.gameObject.name == "Edit" && isInteractable == true)
        {
            isInteractable = false;
            btn.interactable = false;
            GameManager.Instance.EditSelect();
        }
    }
}
