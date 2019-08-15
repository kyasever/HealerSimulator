using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class DBMPanel : MonoBehaviour
{
    private ObjectPool pool;

    private void Awake()
    {
        pool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMode.Instance.Boss == null)
            return;
        Refresh(GameMode.Instance.Boss);
    }

    public void Refresh(Character c)
    {
        int count = c.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<DBMHUD>().RefreshSkill(c.SkillList[i]);
        }
    }
}
