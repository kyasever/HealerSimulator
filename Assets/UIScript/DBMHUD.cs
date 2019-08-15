using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class DBMHUD : MonoBehaviour
{
    [HideInInspector]
    public KSlider Slider;
    public Text NameLabel;
    public Text AmountLabel;

    private void Awake()
    {
        Slider = GetComponent<KSlider>();
    }

    public void RefreshSkill(Skill s)
    {
        NameLabel.text = s.skillName;
        AmountLabel.text = Utils.GetNString(s.CDRelease, s.CD);
        Slider.FillAmount = (float)s.CDRelease / s.CD;
    }
}
