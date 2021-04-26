using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    //Sliders
    private Slider pressSlider;
    private Slider cutSlider;
    
    //
    [SerializeField] private GameObject pressMachine;
    [SerializeField] private GameObject recordWithRing;
    
    //Values
    private float pressVerticalValue;
    private float cutRotValue;
    
    //bools
    private bool pressAtMax = false;
    private bool cutmachineAtMax = false;

    private void Start()
    {
        pressSlider = GameObject.Find("Slider_Press").GetComponent<Slider>();
        cutSlider = GameObject.Find("Slider_Cut").GetComponent<Slider>();
    }
    
    private void Update()
    {
        CalculatePressSliderValue();
        CalculateCutMachineSliderValue();
        print(cutRotValue);
    }

    private void CalculatePressSliderValue()
    {
        if (!pressAtMax)
        {
            pressVerticalValue = pressMachine.transform.position.y * -1;
            pressSlider.value = pressVerticalValue;
        }
        if (pressVerticalValue >= -1.607f)
        {
            pressAtMax = true;
            pressSlider.value = -1.607f;
        }
        else
        {
            pressAtMax = false;
        }
    }

    private void CalculateCutMachineSliderValue()
    {
        
        if (!cutmachineAtMax)
        {
            cutRotValue = recordWithRing.transform.eulerAngles.y;
            cutSlider.value = cutRotValue;
        }
        if (cutRotValue >= 359)
        {
            cutmachineAtMax = true;
            cutSlider.value = 359f;
        }
        else
        {
            cutmachineAtMax = false;
        }
    }
}
