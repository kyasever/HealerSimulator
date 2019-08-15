using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterHUD : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
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

    public Color selectColor; 
    public Color unSelectColor;

    // Update is called once per frame
    void Update()
    {
        if (sourceCharacter != null)
        {
            Refresh(sourceCharacter);
        }
    }

    public void Refresh(Character character)
    {
        nameLabel.text = character.CharacterName;
        hpLabel.text = Utils.GetNString(character.HP, character.MaxHP);
        hpSlider.Value = character.HP;
        hpSlider.MaxValue = character.MaxHP;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameMode.Instance.FocusCharacter != sourceCharacter && sourceCharacter.IsAlive)
        {
            GameMode.Instance.FocusCharacter = sourceCharacter;
        }
        panelImage.color = selectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panelImage.color = unSelectColor;
    }
}