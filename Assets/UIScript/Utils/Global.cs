using HealerSimulator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//这个类是GameMode的驱动类. GameMode本身不进入Unity循环. 使用这个进行驱动
public class Global : MonoBehaviour
{
    public static Global Instance;

    //static Global()
    //{
    //    GameObject go = new GameObject("Global");
    //    DontDestroyOnLoad(go);
    //    Instance = go.AddComponent<Global>();
    //}


    private GameMode game;

    public int ControllerNum;

    public int TeamCharacterCount;

    public int DeathCharacterCount;

    public GameObject StartCanvas;

    public GameEndPanel GameEndPanel;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        game = GameMode.Instance;
        Screen.SetResolution(1024, 768, false);
        Screen.fullScreen = false;
    }


    public void NewGame(int diff)
    {
        game.InitGame(diff);
        StartCanvas.SetActive(false);
        game.InBattle = true;
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        StartCanvas.SetActive(true);
        game.InBattle = false;
        //SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }


    private float updateTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (game.UpdateEvent != null)
            ControllerNum = game.UpdateEvent.GetInvocationList().Length;
        else
            ControllerNum = 0;
        if(game.TeamCharacters != null)
            TeamCharacterCount = game.TeamCharacters.Count;

        if (game.InBattle)
            game.BattleTime += Time.deltaTime;

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
