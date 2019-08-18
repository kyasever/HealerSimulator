using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterHUD : DataBinding<Character> , IPointerEnterHandler , IPointerExitHandler
{
    public Transform damegeTextTrans;

    private ObjectPool pool;

    private Coroutine coroutine;

    private GameObject obj;

    public void OnHit(SkadaRecord record)
    {
        //如果正在执行一个协程 那么中止它并把对象返回
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            pool.ReturnInstantiate(obj);
        }

        obj = pool.GetInstantiate();
        obj.transform.SetParent(damegeTextTrans);
        obj.transform.localPosition = Vector3.zero;

        Text text = obj.GetComponent<Text>();
        if(record.Value > 0)
        {
            text.color = Color.blue;
            text.text = "+" + record.Value.ToString();
        }
        else
        {
            text.color = Color.red;
            text.text = record.Value.ToString();
        }
        coroutine =  StartCoroutine(Utils.FadeAndUp(obj.GetComponent<CanvasGroup>(), 0f, 1.5f,20f,pool));
    }


    public Text nameLabel;
    public Text hpLabel;
    public KSlider hpSlider;

    private Image panelImage;

    void Awake()
    {
        panelImage = GetComponent<Image>();
        pool = GetComponent<ObjectPool>();
    }

    public Color selectColor; 
    public Color unSelectColor;

    public override void Refresh()
    {
        Character character = sourceData;

        if(character.BehitRecord != null)
        {
            OnHit(character.BehitRecord);
            character.BehitRecord = null;
        }

        nameLabel.text = character.CharacterName;
        hpLabel.text = Utils.GetNString(character.HP, character.MaxHP);
        //先设定va 再设定 max 自然 val 就没有设定上去了
        hpSlider.MaxValue = character.MaxHP;
        hpSlider.Value = character.HP;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameMode.Instance.FocusCharacter != sourceData && sourceData.IsAlive)
        {
            GameMode.Instance.FocusCharacter = sourceData;
        }
        panelImage.color = selectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        panelImage.color = unSelectColor;
    }
}