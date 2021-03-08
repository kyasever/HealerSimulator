using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;

public class StageMsgPanel : MonoBehaviour
{
    private GameMode game;
    // Start is called before the first frame update
    void Start()
    {
        GameMode.Instance.Connect(Refresh, Lifecycle.UIUpdate);
    }

    void Refresh(GameMode gameMode)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        string s = (gameMode.DifficultyLevel * 10).ToString() + "%";
        StringBuilder sb = new StringBuilder().AppendFormat("{0}  游戏难度:{1}  难度加成:{2}", game.LevelName, game.DifficultyLevel, s);
        GetComponent<Text>().text = sb.ToString();
    }

}
