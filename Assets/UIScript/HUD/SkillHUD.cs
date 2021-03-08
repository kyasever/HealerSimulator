using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;
    public Text SkillNameLabel;
    public Text KeyTextLabel;
    public Text ReleaseTimeLabel;
    public Text TooltipLeftTopLabel;
    public Text TooltipRightTopLabel;
    public Text TooltipBottomLabel;

    public CanvasGroup TooltipCanvas;

    void Start()
    {
        TooltipCanvas.alpha = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Utils.Fade(TooltipCanvas, 1.0f, 0.5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Utils.Fade(TooltipCanvas, 0f, 0.5f));
    }

    public void Refresh(Skill s)
    {
        //KeyTextLabel.text = s.Key.ToString();
        SkillNameLabel.text = s.skillName;
        KeyTextLabel.text = s.Key.ToString();
        TooltipLeftTopLabel.text = s.skillName;
        TooltipRightTopLabel.text = s.MPCost.ToString();
        TooltipBottomLabel.text = s.skillDescription;
        //这是一个走cd技能,当进入cd的时候,转圈显示的是技能的CD
        if (s.CDRelease >= 0)
        {
            ReleaseTimeLabel.gameObject.SetActive(true);
            ReleaseTimeLabel.text = s.CDRelease.ToString("F1");
            Icon.fillAmount = 1 - s.CDRelease / s.CD;
        }
        else
        {
            ReleaseTimeLabel.gameObject.SetActive(false);
            Icon.fillAmount = 1 - (s.Caster.CommonTime / s.Caster.CommonInterval);
        }

        if (!s.CanCast)
        {
            ReleaseTimeLabel.gameObject.SetActive(false);
            Icon.fillAmount = 0;
        }

    }
}
