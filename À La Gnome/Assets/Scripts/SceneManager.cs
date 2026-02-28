using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    public Camera cam;
    public Image fadeImage;

    public Transform setting1Pos;
    public Transform setting2Pos;
    public Transform setting3Pos;

    public float fadeDuration = 0.4f;
    public float slideDuration = 0.5f;

    private int currentSetting = 1;
    private bool isTransitioning = false;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.UI.Select1.performed += ctx => ChangeSetting(1);
        controls.UI.Select2.performed += ctx => ChangeSetting(2);
        controls.UI.Select3.performed += ctx => ChangeSetting(3);
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }

    void ChangeSetting(int newSetting)
    {
        if (newSetting == currentSetting) return;

        if ((currentSetting == 2 && newSetting == 3) ||
            (currentSetting == 3 && newSetting == 2))
        {
            StartCoroutine(SlideTransition(newSetting));
        }
        else
        {
            StartCoroutine(FadeTransition(newSetting));
        }
    }

    IEnumerator FadeTransition(int newSetting)
    {
        isTransitioning = true;
        yield return StartCoroutine(Fade(1));
        MoveCameraInstant(newSetting);
        yield return StartCoroutine(Fade(0));

        currentSetting = newSetting;
        isTransitioning = false;
    }

    IEnumerator SlideTransition(int newSetting)
    {
        isTransitioning = true;

        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = GetPosition(newSetting);

        float time = 0;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float t = time / slideDuration;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        cam.transform.position = targetPos;

        currentSetting = newSetting;
        isTransitioning = false;
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime; ;
            float t = time / fadeDuration;

            Color c = fadeImage.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = c;

            yield return null;
        }

        Color final = fadeImage.color;
        final.a = targetAlpha;
        fadeImage.color = final;
    }

    void MoveCameraInstant(int setting)
    {
        cam.transform.position = GetPosition(setting);
    }

    Vector3 GetPosition(int setting)
    {
        switch (setting)
        {
            case 1: return setting1Pos.position;
            case 2: return setting2Pos.position;
            case 3: return setting3Pos.position;
        }

        return cam.transform.position;
    }
}


