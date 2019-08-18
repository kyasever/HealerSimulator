using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class DBMHUD : DataBinding<Skill>
{
    [HideInInspector]
    public KSlider Slider;
    public Text NameLabel;
    public Text AmountLabel;

    private void Awake()
    {
        Slider = GetComponent<KSlider>();
    }

    public override void Refresh()
    {
        Skill s = sourceData;

        StringBuilder sb = new StringBuilder().Append(s.skillName).Append("[").Append(s.skillDiscription).Append("]");

        NameLabel.text = sb.ToString();
        AmountLabel.text = Utils.GetNString(s.CDRelease, s.CD);
        Slider.FillAmount = (float)s.CDRelease / s.CD;
    }
}
