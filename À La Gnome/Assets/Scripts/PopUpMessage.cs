using UnityEngine;
using TMPro;
using System.Collections;

public class PopUpMessage : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textTMP;
    public GameObject panel;

    [Header("Settings")]
    public float displayTime = 3f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (panel == null || textTMP == null) return;

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        textTMP.text = message;
        panel.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        panel.SetActive(false);
        currentCoroutine = null;
    }
}
