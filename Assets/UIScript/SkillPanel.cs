using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SkillPanel : DataBinding<Character>
{
    private GameMode game;
    
    //public Character sourceCharacter;

    private ObjectPool pool;

    // Start is called before the first frame update
    void Start()
    {
        game = GameMode.Instance;
        game.OnChangeEvent.Add(()=> { Binding(game.Player); });

        pool = GetComponent<ObjectPool>();
    }


    public override void Refresh()
    {
        Character c = sourceData;
        int count = c.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<SkillHUD>().Binding(c.SkillList[i]);
        }
    }
}
