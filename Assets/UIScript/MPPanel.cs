using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class MPPanel : MonoBehaviour
{
    public Character sourceCharacter;
    public KSlider slider;
    public Text leftLabel;
    public Text rightLabel;


    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (sourceCharacter == null)
        {
            sourceCharacter = GameMode.Instance.Player;
            return;
        }
        Refresh(sourceCharacter);
    }

    public bool IsInFading = false;


    public void Refresh(Character c)
    {
        if (c == null)
        {
            leftLabel.text = "空缺";
            return;
        }

        leftLabel.text = c.CharacterName;
        rightLabel.text = Utils.GetNString(c.MP, c.MaxMP);
        slider.FillAmount = ((float)c.MP / c.MaxMP);
    }
}
