using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private TextMeshProUGUI[] keyTexts;

    public bool IsOpen => panel.activeSelf;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        SettingManager.LoadKeyBindings(inputActions);

        RefreshBindings();
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void RefreshBindings()
    {
        for (int i = 0; i < keyTexts.Length && i < 4; i++)
        {
            InputAction action = inputActions.FindAction($"NoteLine{i + 1}");

            keyTexts[i].text = action != null && action.bindings.Count > 0 ? action.GetBindingDisplayString(0) : "-";
        }
    }
}