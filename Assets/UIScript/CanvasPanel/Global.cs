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

    private GameMode game;

    public GameObject StartCanvas;

    public GameEndPanel GameEndPanel;


    public void EndGameWithBtn()
    {
        game.InBattle = false;
        Global.Instance.GameEndPanel.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        game = GameMode.Instance;
        Screen.SetResolution(1024, 768, false);
        Screen.fullScreen = false;
    }

    public void NewGame(int diff, int level)
    {
        game.InitGame(diff, level);
        StartCanvas.SetActive(false);
        game.InBattle = true;
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void EndGame()
    {
        game.Clear();
        GameEndPanel.gameObject.SetActive(false);

        StartCanvas.SetActive(true);
        game.InBattle = false;
        //SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }


    private float updateTime = 1f;
}
