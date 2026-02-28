using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SliderControl : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider slider;
    public float increaseSpeed = 40f;
    public float decreaseSpeed = 30f;

    [Header("Golden Area")]
    public float goldenMin = 10f;
    public float goldenMax = 100f;

    [Header("Golden Area Panel")]
    public RectTransform goldenPanel;

    [Header("Animation")]
    public Animator animator;
    public string animatorBoolName;

    private bool isHoldingSpace = false;
    private bool isInGoldenArea = false;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 0;
        SetRandomGoldenArea();
        UpdateGoldenPanel();
    }

    private void Update()
    {
        if (isHoldingSpace)
            slider.value += increaseSpeed * Time.deltaTime;
        else
            slider.value -= decreaseSpeed * Time.deltaTime;

        slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);

        bool nowInGolden = slider.value >= goldenMin && slider.value <= goldenMax;

        slider.handleRect.GetComponent<Image>().color = nowInGolden ? Color.yellow : Color.gray;

        if (animator != null)
        {
            animator.SetBool(animatorBoolName, nowInGolden);
        }

        isInGoldenArea = nowInGolden;
    }

    public void OnSpaceHold(InputAction.CallbackContext context)
    {
        if (context.performed)
            isHoldingSpace = true;
        else if (context.canceled)
            isHoldingSpace = false;
    }

    public void SetRandomGoldenArea()
    {
        goldenMin = Random.Range(30f, 50f);
        goldenMax = goldenMin + Random.Range(10f, 40f);
        UpdateGoldenPanel();
    }

    private void UpdateGoldenPanel()
    {
        if (goldenPanel == null || slider == null) return;

        RectTransform sliderRect = slider.GetComponent<RectTransform>();

        float widthFraction = (goldenMax - goldenMin) / (slider.maxValue - slider.minValue);
        float centerFraction = ((goldenMin + goldenMax) / 2 - slider.minValue) / (slider.maxValue - slider.minValue);

        goldenPanel.sizeDelta = new Vector2(sliderRect.rect.width * widthFraction, goldenPanel.sizeDelta.y);

        float localX = sliderRect.rect.width * centerFraction - sliderRect.rect.width / 2;
        goldenPanel.localPosition = new Vector3(localX, goldenPanel.localPosition.y, goldenPanel.localPosition.z);
    }

}
