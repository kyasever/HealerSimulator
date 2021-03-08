using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

//这个应该绑定GameMode,当GameMode状态发生变化的时候,调用Refresh
public class BossHPPanel : MonoBehaviour
{
    public KSlider slider;
    public Text nameLabel;
    public Text disLabel;
    public KSlider sliderCommonCD;

    private GameMode game;

    void Start()
    {
        sliderCommonCD.Value = 0;

        GameMode.Instance.Connect(Refresh, Lifecycle.UIUpdate);
    }

    void Refresh(GameMode gameMode)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        Character c = gameMode.Player;
        slider.Value = c.HP;
        slider.MaxValue = c.MaxHP;
        nameLabel.text = c.CharacterName;
        disLabel.text = Utils.GetNString(c.HP, c.MaxHP);
    }

}
