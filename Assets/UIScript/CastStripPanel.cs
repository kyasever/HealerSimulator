using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class CastStripPanel : MonoBehaviour
{
    public Character sourceCharacter;
    public KSlider slider;
    public Text nameLabel;
    public Text timeLabel;
    public KSlider sliderCommonCD;


    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(sourceCharacter == null)
        {
            sourceCharacter = GameMode.Instance.Player;
            return;
        }
        Refresh(sourceCharacter);
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
