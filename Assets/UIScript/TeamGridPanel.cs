using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;

public class TeamGridPanel : MonoBehaviour
{
    private GameMode game;
    private ObjectPool pool;


    void Start()
    {
        pool = GetComponent<ObjectPool>();
        GameMode.Instance.Connect(Refresh, Lifecycle.UIUpdate);
    }

    void Refresh(GameMode gameMode)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        int count = game.TeamCharacters.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<CharacterHUD>().Refresh(game.TeamCharacters[i]);
        }
    }

}