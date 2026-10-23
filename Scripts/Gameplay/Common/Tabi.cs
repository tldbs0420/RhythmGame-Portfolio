using UnityEngine;

public class Tabi : MonoBehaviour
{
    public static Tabi Active { get; private set; }

    public Animator anim;

    [SerializeField] private float idleWait = 5f;

    private float lastAnimTime;
    private int pressedMask = 0;

    private void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        lastAnimTime = Time.time;
    }

    private void OnEnable()
    {
        Active = this;
    }

    private void OnDisable()
    {
        if (Active == this)
            Active = null;
    }

    // 노트 입력 시 InputManager에서 호출
    public void OnNotePressed(int lineIndex)
    {
        if (lineIndex < 0 || lineIndex > 3)
            return;

        pressedMask |= 1 << lineIndex;
    }

    private void Update()
    {
        if (anim == null)
            return;

        anim.SetBool("leftPressed", (pressedMask & 1) != 0);
        anim.SetBool("downPressed", (pressedMask & 2) != 0);
        anim.SetBool("upPressed", (pressedMask & 4) != 0);
        anim.SetBool("rightPressed", (pressedMask & 8) != 0);

        if (pressedMask != 0)
        {
            anim.SetBool("isIdle", false);
            lastAnimTime = Time.time;
        }
        else if (Time.time - lastAnimTime >= idleWait)
        {
            anim.SetBool("isIdle", true);
        }

        pressedMask = 0;
    }
}
