using System;
using TMPro;
using UnityEngine;

public class UI_ResultsPanel : MonoBehaviour
{
    private TextMeshProUGUI[] textMeshes;

    private void OnEnable()
    {
        EventBus.Subscribe("actionResult", OnActionResult);
    }

    private void Awake()
    {
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("actionResult",  OnActionResult);
    }

    private void OnActionResult(object obj)
    {
        PlayerResult result = (PlayerResult)obj;
        textMeshes[result.Index].text = result.ToString();
    }
}
