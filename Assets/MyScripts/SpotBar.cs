using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    PlayerStatusControl playerStatus;

    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusControl>();
        slider.value = 0;
        fill.color = fill.color = gradient.Evaluate(0f);
    }


    void Update()
    {
        SetTimeToSpot();
        
    }


    // Start is called before the first frame update
    public void SetTimeToSpot()
    {
        slider.maxValue = playerStatus.currentTimeToSpot;
       // slider.value = 0;

        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void SetCurrentTime(float currentTime)
    {
        slider.value = currentTime;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}