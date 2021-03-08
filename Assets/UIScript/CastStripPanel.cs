using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主要的问题在于没有分级,要是有一层子脚本绑定SKill就好办了 一层绑Character 一层绑Skill
/// </summary>
public class CastStripPanel : MonoBehaviour
{
    public KSlider slider;
    public Text nameLabel;
    public Text timeLabel;
    public KSlider sliderCommonCD;
    private CanvasGroup canvasGroup;
    private GameMode game;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        GameMode.Instance.Connect(Refresh, Lifecycle.UIUpdate);
    }

    void Refresh(GameMode gameMode)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        Character c = gameMode.Player;
        Skill s = c.CastingSkill;
        if (s == null)
        {
            //如果条读完了,那么不改变字符,渐隐藏起来
            if (Utils.FloatEqual(canvasGroup.alpha, 1))
            {
                slider.Value = slider.MaxValue;
                StartCoroutine(Utils.Fade(canvasGroup, 0f, 1f));
            }
            return;
        }
        //如果现在已经被隐形了,那么马上显示
        if (!Utils.FloatEqual(canvasGroup.alpha, 1))
        {
            canvasGroup.alpha = 1f;
        }
        nameLabel.text = s.skillName;
        float duringTime = s.CastingInterval - s.CastingRelease;
        timeLabel.text = Utils.GetNString(duringTime, s.CastingInterval);
        slider.Value = duringTime;
        slider.MaxValue = s.CastingInterval;

        sliderCommonCD.Value = c.CommonTime;
        sliderCommonCD.MaxValue = c.CommonInterval;
    }
}
