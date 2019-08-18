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
        game = GameMode.Instance;
        game.OnChangeEvent.Add(Refresh);
        pool = GetComponent<ObjectPool>();
    }

    private void Refresh()
    {
        int count = game.TeamCharacters.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for(int i = 0;i<count;i++)
        {
            list[i].GetComponent<CharacterHUD>().Binding(game.TeamCharacters[i]);
        }
    }
}