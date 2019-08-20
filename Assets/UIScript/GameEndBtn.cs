using HealerSimulator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndBtn : MonoBehaviour
{
    private GameMode game;

    private void Start()
    {
        game = GameMode.Instance;
    }

    public void EndGameWithBtn()
    {
        game.InBattle = false;
        Global.Instance.GameEndPanel.gameObject.SetActive(true);
    }

    public void ReverseBattle()
    {
        game.InBattle = !game.InBattle;
    }
}
