using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SkillPanel : MonoBehaviour
{
    public Character sourceCharacter;

    private ObjectPool pool;

    // Start is called before the first frame update
    void Start()
    {
        pool = GetComponent<ObjectPool>();

    }

    // Update is called once per frame
    void Update()
    {
        if (sourceCharacter == null)
        {
            sourceCharacter = GameMode.Instance.Player;
            return;
        }
        Refresh();
    }

    private void Refresh()
    {
        int count = sourceCharacter.SkillList.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for (int i = 0; i < count; i++)
        {
            list[i].GetComponent<SkillHUD>().sourceSkill = sourceCharacter.SkillList[i];
        }
    }
}
