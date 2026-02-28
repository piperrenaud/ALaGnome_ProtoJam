using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    [Header("References")]
    public SliderControl sliderControl;
    public float requiredTimeForActivity = 10f;
    public PopUpMessage popUpMessage;

    private float timer = 0f;
    private bool activityDone = false;

    private void Start()
    {
        sliderControl.slider.gameObject.SetActive(true);
    }

    void Update()
    {
        if (activityDone) return;

        bool inGolden = sliderControl.slider.value >= sliderControl.goldenMin &&
                        sliderControl.slider.value <= sliderControl.goldenMax;

        if (inGolden)
        {
            timer += Time.deltaTime;
        }

        if (timer >= requiredTimeForActivity)
        {
            ActivityComplete();
        }
    }

    private void ActivityComplete()
    {
        activityDone = true;
        sliderControl.enabled = false;
        
        if (sliderControl.animator != null)
        {
            sliderControl.animator.SetBool(sliderControl.animatorBoolName, false);
        }

        sliderControl.slider.gameObject.SetActive(false);
        gameObject.SetActive(false);

        popUpMessage.ShowMessage("Mixing Complete!");
    }

    public void ResetActivity()
    {
        activityDone = false;
        timer = 0f;
        sliderControl.enabled = true;
        sliderControl.slider.value = 0f;
    }
}
