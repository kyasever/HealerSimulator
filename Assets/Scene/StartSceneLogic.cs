using System;
using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneLogic : MonoBehaviour
{
    public Dropdown Dropdown;
    public InputField Field;
    public Button Button;

    // Start is called before the first frame update
    void Start()
    {
        Dropdown.options = new List<Dropdown.OptionData>() { new Dropdown.OptionData("关卡1") };
        Button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        Global.Instance.NewGame(Convert.ToInt32(Field.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
