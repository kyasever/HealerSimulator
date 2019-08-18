using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class DBMPanel : MonoBehaviour
{
    private ObjectPool pool;

    private GameMode game;

    private void Start()
    {
        game = GameMode.Instance;
        game.OnChangeEvent.Add(Refresh);

        pool = GetComponent<ObjectPool>();
    }

    public void Refresh()
    {
        int count = game.Boss.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<DBMHUD>().Binding(game.Boss.SkillList[i]);
        }
    }
}
