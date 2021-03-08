using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FocusHUD : MonoBehaviour
{
    public Text nameLabel;
    public Text hpLabel;
    public KSlider hpSlider;

    private Image panelImage;

    void Start()
    {
        panelImage = GetComponent<Image>();
        GameMode.Instance.Connect((GameMode game) =>
        {
            var sourceData = GameMode.Instance.FocusCharacter;
            nameLabel.text = sourceData.CharacterName;
            hpLabel.text = Utils.GetNString(sourceData.HP, sourceData.MaxHP);
            hpSlider.Value = sourceData.HP;
            hpSlider.MaxValue = sourceData.MaxHP;
        }, Lifecycle.UIUpdate);
    }
}