using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BUFFHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;

    public Text ReleaseTimeLabel;

    public Text TooltipLabel;

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

    public void Refresh(BUFF buff)
    {
        ReleaseTimeLabel.text = buff.ReleaseTime.ToString("F1");
        Icon.fillAmount = buff.ReleaseTime / buff.DefaultTime;

        if (buff.IsPositive)
        {
            Icon.color = Color.green;
        }
        else
        {
            Icon.color = Color.red;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0}: {1}", buff.Name, buff.Description);
        TooltipLabel.text = sb.ToString();
    }
}
