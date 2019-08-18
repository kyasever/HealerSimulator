using HealerSimulator;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class NPCContainer
{
    public Transform Trans;
    public List<NPCHUD> list = new List<NPCHUD>();

    private GameManager manager;

    public NPCContainer(Transform trans,GameManager manager)
    {
        Trans = trans;
        this.manager = manager;
    }

    public void Add(NPCHUD npc)
    {
        list.Add(npc);
        npc.transform.parent = Trans;
    }

    public void Remove(NPCHUD npc)
    {
        list.Remove(npc);
    }

    public List<NPCData> ToDataList()
    {
        List<NPCData> data = new List<NPCData>();
        foreach(var v in list)
        {
            data.Add(v.ToData());
        }
        return data;
    }

    public void Parse(List<NPCData> data)
    {
        list = new List<NPCHUD>();
        foreach(var v in data)
        {
            GameObject obj = manager.pool.GetInstantiate();
            NPCHUD npc = obj.GetComponent<NPCHUD>();
            npc.ParseData(v);
            Add(npc);
        }
    }
}

public class SaveData
{
    public int Coins;
    public int Exp;

    public List<NPCData> ShopList;
    public List<NPCData> BagList;
    public List<NPCData> TeamList;

    /// <summary>
    /// 创建一个存档类
    /// </summary>
    public static SaveData CreateSave(GameManager gameData)
    {
        SaveData save = new SaveData
        {
            Coins = gameData.Coins,
            Exp = gameData.Exp,
            ShopList = gameData.ShopContainer.ToDataList(),
            BagList = gameData.BagContainer.ToDataList(),
            TeamList = gameData.TeamContainer.ToDataList()
        };
        return save;
    }

    /// <summary>
    /// 读取一个存档类
    /// </summary>
    public static void Load(GameManager gameData,SaveData save)
    {
        gameData.Coins = save.Coins;
        gameData.Exp = save.Exp;
        gameData.ShopContainer.Parse(save.ShopList);
        gameData.BagContainer.Parse(save.BagList);
        gameData.TeamContainer.Parse(save.TeamList);
    }
}

public class NPCData
{
    public int hp;
    public int dpsMin;
    public int dpsMax;
    public TeamDuty duty;
    public float defense;
    //特性是被动技能 等什么时候添加了被动技能再说吧 被动技能考虑新建新Skill类型. 然后由Controller控制. 基于脚本触发式被动
}

/// <summary>
/// 这里保存着角色的存档信息
/// </summary>
public class GameManager : MonoBehaviour
{
    public ObjectPool pool;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
        pool = GetComponent<ObjectPool>();
        ShopContainer = new NPCContainer(ShopTrans,this);
        BagContainer = new NPCContainer(BagTrans,this);
        TeamContainer = new NPCContainer(TeamTrans,this);
    }


    public int Coins = 10000;
    public int Exp = 10000;

    public Transform ShopTrans;
    public Transform BagTrans;
    public Transform TeamTrans;

    public NPCContainer ShopContainer;
    public NPCContainer BagContainer;
    public NPCContainer TeamContainer;


    public void SaveGame()
    {
        SaveData data = SaveData.CreateSave(this);
        string str = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/gamesave.save", str);
        Debug.Log(("存档地址:" + Application.persistentDataPath + "/gamesave.save"));
        Debug.Log(("游戏已保存"));
    }

    public void LoadGame()
    {
        Debug.Log("读取地址:" + Application.persistentDataPath + "/gamesave.save");
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            SaveData save = new SaveData();
            string str = File.ReadAllText(Application.persistentDataPath + "/gamesave.save");
            save = JsonUtility.FromJson<SaveData>(str);

            SaveData.Load(this, save);

            Debug.Log("游戏已载入");
        }
        else
        {
            Debug.Log("读取存档失败,开始新游戏");
        }
    }
}
