using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KSlider : MonoBehaviour
{
    public Image image;

    private float v = 0f;
    public float Value { get { return v; }
        set {
            v = value;
            if (v < 0)
                v = 0;
            if (v > maxValue)
                v = maxValue;
            Set();
        } }
    private float maxValue = 100f;
    public float MaxValue { get { return maxValue; } set { this.maxValue = value; Set(); } }

    public float FillAmount
    {
        get
        {
            return image.fillAmount;
        }
        set
        {
            image.fillAmount = value;
        }
    }
     

    private void Awake()
    {
    }

    private void Set()
    {
        image.fillAmount = v / maxValue;
    }
}
