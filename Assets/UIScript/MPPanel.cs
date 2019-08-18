using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class MPPanel : DataBinding<Character>
{
    private GameMode game;

    public KSlider slider;
    public Text leftLabel;
    public Text rightLabel;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Start()
    {
        game = GameMode.Instance;
        game.OnChangeEvent.Add(()=> { Binding(game.Player); });

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public bool IsInFading = false;

    public override void Refresh()
    {
        Character c = sourceData;
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
