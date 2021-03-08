using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    public void Refresh(Skill s)
    {
        StringBuilder sb = new StringBuilder().Append(s.skillName).Append("[").Append(s.skillDescription).Append("]");

        NameLabel.text = sb.ToString();
        AmountLabel.text = Utils.GetNString(s.CDRelease, s.CD);
        Slider.FillAmount = (float)s.CDRelease / s.CD;
    }
}
