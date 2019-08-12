using HealerSimulator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个类是GameMode的驱动类. GameMode本身不进入Unity循环. 使用这个进行驱动
public class GameLoop : MonoBehaviour
{
    private GameMode game;

    public int ControllerNum { get => game.UpdateEvent.GetInvocationList().Length; }

    public int TeamCharacterCount { get => game.TeamCharacters.Count; }

    public int DeathCharacterCount { get => game.DeadCharacters.Count; }


    // Start is called before the first frame update
    void Awake()
    {
        game = GameMode.Instance;
        game.InitGame(5);
    }

    public float updateTime = 1f;

    // Update is called once per frame
    void Update()
    {
        game.UpdateEvent?.Invoke();

        //每秒钟更新一次
        updateTime -= Time.deltaTime;
        if (updateTime < 0)
        {
            updateTime = 1f;
            game.UpdatePerSecendEvent?.Invoke();
        }
    }
}
