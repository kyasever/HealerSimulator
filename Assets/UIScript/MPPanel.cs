using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class MPPanel : MonoBehaviour
{
    private GameMode game;

    public KSlider slider;
    public Text leftLabel;
    public Text rightLabel;
    private CanvasGroup canvasGroup;
    public bool IsInFading = false;

    // Start is called before the first frame update
    private void Start()
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
