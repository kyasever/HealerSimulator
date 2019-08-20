using System;
using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public Dropdown Dropdown;
    public InputField Field;
    public Button Button;

    // Start is called before the first frame update
    void Start()
    {
        Dropdown.options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("BOSS1"), new Dropdown.OptionData("BOSS2") };
        Dropdown.value = 1;
        Button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        int diff = 0;
        int level = 1;
        try
        {
            diff = Convert.ToInt32(Field.text);
            level = Dropdown.value+1;
            Global.Instance.NewGame(Convert.ToInt32(Field.text), level);
        }
        catch
        {
            Debug.LogError("输入不合法");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
