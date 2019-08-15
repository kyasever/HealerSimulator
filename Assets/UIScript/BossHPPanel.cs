using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class BossHPPanel : MonoBehaviour
{
    public Character sourceCharacter;
    public KSlider slider;
    public Text nameLabel;
    public Text disLabel;
    public KSlider sliderCommonCD;

    // Start is called before the first frame update
    void Start()
    {
        sourceCharacter = GameMode.Instance.Boss;
        sliderCommonCD.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(sourceCharacter == null)
        {
            sourceCharacter = GameMode.Instance.Boss;
        }
        if(sourceCharacter != null)
        {
            Refresh(sourceCharacter);
        }
    }

    public void Refresh(Character c)
    {
        slider.Value = c.HP;
        slider.MaxValue = c.MaxHP;
        nameLabel.text = c.CharacterName;
        disLabel.text = Utils.GetNString(c.HP, c.MaxHP);
    }

}
