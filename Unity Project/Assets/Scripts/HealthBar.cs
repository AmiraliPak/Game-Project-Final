using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxValue(float mxvalue)
    {
        slider.maxValue = mxvalue;
        slider.value = mxvalue;
        fill.color = gradient.Evaluate(1f);

    }

    public void SetValue(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
