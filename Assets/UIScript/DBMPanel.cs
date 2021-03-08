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
        pool = GetComponent<ObjectPool>();

        GameMode.Instance.Connect(Refresh, Lifecycle.UIUpdate);
    }

    void Refresh(GameMode gameMode)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        int count = gameMode.Boss.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<DBMHUD>().Refresh(game.Boss.SkillList[i]);
        }
    }

}
