using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

//这个应该绑定GameMode,当GameMode状态发生变化的时候,调用Refresh
public class BossHPPanel : DataBinding<Character>
{
    public KSlider slider;
    public Text nameLabel;
    public Text disLabel;
    public KSlider sliderCommonCD;

    private GameMode game;

    // Start is called before the first frame update
    void Start()
    {
        sliderCommonCD.Value = 0;

        game = GameMode.Instance;
        game.OnChangeEvent.Add(() => { Binding(game.Boss); });
    }

    //这个方法是没问题的. character添加自己的回调给这个
    public override void Refresh()
    {
        Character c = sourceData;
        slider.Value = c.HP;
        slider.MaxValue = c.MaxHP;
        nameLabel.text = c.CharacterName;
        disLabel.text = Utils.GetNString(c.HP, c.MaxHP);
    }

}
