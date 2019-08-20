using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 这个绑定的是一条skada数据,不属于绑定核心方法
/// </summary>
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
