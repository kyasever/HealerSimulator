using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;

public class TeamGridPanel : MonoBehaviour
{
    private ObjectPool pool;

    private GameMode game = GameMode.Instance;

    void Start()
    {
        pool = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        int count = game.TeamCharacters.Count;
        List<GameObject> list = pool.GetInstantiate(count);
        for(int i = 0;i<count;i++)
        {
            list[i].GetComponent<CharacterHUD>().sourceCharacter = game.TeamCharacters[i];
        }
    }
}