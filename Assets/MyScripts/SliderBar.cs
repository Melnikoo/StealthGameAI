using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    // Start is called before the first frame update
    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = 0;

        fill.color = gradient.Evaluate(0f);
    }
    public void SetValue(float value )
    {
        slider.value = value;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
