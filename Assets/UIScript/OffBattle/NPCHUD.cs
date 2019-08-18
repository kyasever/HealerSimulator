using HealerSimulator;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NPCHUD: MonoBehaviour
{
    /// <summary>
    /// 转换为Data
    /// </summary>
    public NPCData ToData()
    {
        return new NPCData();
    }

    /// <summary>
    /// 从Data解析
    /// </summary>
    public void ParseData(NPCData data)
    {

    }


    public Button BtnBuy;
    public Button BtnReload;
    public Button BtnInTeam;
    public Button BtnOutTeam;
    public Button BtnDelete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
