using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SkillPanel : MonoBehaviour
{
    private GameMode game;

    //public Character sourceCharacter;

    private ObjectPool pool;

    // Start is called before the first frame update
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
        Character c = gameMode.Player;
        int count = c.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<SkillHUD>().Refresh(c.SkillList[i]);
        }
    }

}
