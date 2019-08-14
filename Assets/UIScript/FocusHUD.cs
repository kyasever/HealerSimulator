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
    public Character sourceCharacter;

    public Text nameLabel;
    public Text hpLabel;
    public KSlider hpSlider;

    private Image panelImage;

    void Awake()
    {
        panelImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        sourceCharacter = GameMode.Instance.FocusCharacter;
        if (sourceCharacter != null)
        {
            Refresh(sourceCharacter);
        }
    }

    public void Refresh(Character character)
    {
        nameLabel.text = sourceCharacter.CharacterName;
        hpLabel.text = Utils.GetNString(sourceCharacter.HP, sourceCharacter.MaxHP);
        hpSlider.Value = sourceCharacter.HP;
        hpSlider.MaxValue = sourceCharacter.MaxHP;
    }
}