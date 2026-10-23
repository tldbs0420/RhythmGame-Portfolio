using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneKeyGuide : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private TextMeshProUGUI[] keyTexts;
    [SerializeField] private CanvasGroup canvasGroup;

    private IEnumerator Start()
    {
        SettingManager.LoadKeyBindings(playerInput.actions);

        for (int i = 0; i < keyTexts.Length; i++)
        {
            InputAction action = playerInput.actions.FindAction($"NoteLine{i + 1}");

            keyTexts[i].text = action != null && action.bindings.Count > 0 ? action.GetBindingDisplayString(0) : "-";
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(1.5f);

        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / 0.5f);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}