using HealerSimulator;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEndPanel : MonoBehaviour
{
    public Text labelUp;
    public Text labelLeft;
    public Text labelRight;
    public Button button;

    private GameMode game;

    public Dictionary<string, string> Data;

    public void DisplayGameEndPanel()
    {
        if (game.Boss.HP == 0)
            labelUp.text = "游戏胜利";
        else
            labelUp.text = "游戏失败";


        Data = new Dictionary<string, string>();

        Data.Add("游戏难度", game.DifficultyLevel.ToString());
        Data.Add("战斗时长", game.BattleTime.ToString("F1")+"s");

        float f = (game.Boss.HP / (float)game.Boss.MaxHP) * 100;
        StringBuilder sb = new StringBuilder().Append(game.Boss.HP.ToString()).Append("(");
        sb.Append(f.ToString("F1")).Append("%)");
        Data.Add("BOSS剩余血量", sb.ToString());

        int teamHP = 0;
        foreach(var v in game.TeamCharacters)
        {
            teamHP += v.HP;
        }
        Data.Add("团队剩余血量", teamHP.ToString());

        var skadaData = Skada.Instance.GetData(game.Player);

        sb = new StringBuilder().Append(skadaData.Heal.ToString()).Append(" | ");
        sb.Append((skadaData.Heal / game.BattleTime).ToString("F1")).Append("/s");

        Data.Add("玩家输出治疗量", sb.ToString());
        Data.Add("玩家输出伤害", skadaData.Damage.ToString());
        Data.Add("玩家受到伤害", Mathf.Abs(skadaData.BeDamaged).ToString());

        StringBuilder left = new StringBuilder();
        StringBuilder right = new StringBuilder();

        foreach(var kv in Data)
        {
            left.Append(kv.Key).Append("\n");
            right.Append(kv.Value).Append("\n");
        }

        labelLeft.text = left.ToString();
        labelRight.text = right.ToString();
    }

    private void OnEnable()
    {
        game = GameMode.Instance;
        button.onClick.AddListener(ReturnScene);
        DisplayGameEndPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReturnScene()
    {
        game.Clear();
        Global.Instance.EndGame();
    }
}
