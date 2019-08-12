using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class SkadaHUD : MonoBehaviour
{
    [HideInInspector]
    public KSlider Slider;
    public Text NameLabel;
    public Text AmountLabel;

    private void Awake()
    {
        Slider = GetComponent<KSlider>();
    }
}
