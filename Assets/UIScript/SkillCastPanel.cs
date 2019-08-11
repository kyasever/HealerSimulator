using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class SkillCastPanel : MonoBehaviour
{
    public Character sourceCharacter;
    public KSlider slider;
    public Text nameLabel;
    public Text timeLabel;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        sourceCharacter = GameMode.Instance.Player;
    }

    // Update is called once per frame
    private void Update()
    {
        if (sourceCharacter != null)
        {
            Refresh(sourceCharacter);
        }
    }

    public bool IsInFading = false;


    public void Refresh(Character c)
    {
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
        if (!Utils.FloatEqual(canvasGroup.alpha, 1))
        {
            canvasGroup.alpha = 1f;
        }
        nameLabel.text = s.skillName;
        float duringTime = s.CastingInterval - s.CastingRelease;
        timeLabel.text = Utils.GetNString(duringTime, s.CastingInterval);
        slider.Value = duringTime;
        slider.MaxValue = s.CastingInterval;
    }
}
