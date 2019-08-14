using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class SkadaPanel : MonoBehaviour
{
    public GameObject SkadaHUDPrefeb;

    public float updateTime = 1f;

    public List<SkadaHUD> skadaHUDs = new List<SkadaHUD>();

    private void Start()
    {
        for(int i = 0;i<10;i++)
        {
            var skadaHUD = Instantiate(SkadaHUDPrefeb, transform).GetComponent<SkadaHUD>();
            skadaHUD.gameObject.SetActive(false);
            skadaHUDs.Add(skadaHUD);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //每秒钟更新一次
        updateTime -= Time.deltaTime;
        if(updateTime < 0)
        {
            updateTime = 1f;
            Refresh();
        }

    }

    public void Refresh()
    {
        var list = Skada.Instance.GetDamageRank();
        if (list.Count == 0)
        {
            return;
        }
        int i = 0;

        int maxData = list[0].Value.Damage;
        if(maxData == 0)
        {
            maxData = 1;
        }

        foreach(var p in list)
        {
            skadaHUDs[i].gameObject.SetActive(true);
            skadaHUDs[i].NameLabel.text = p.Key.CharacterName;
            skadaHUDs[i].AmountLabel.text = (p.Value.Damage).ToString();
            skadaHUDs[i].Slider.FillAmount = (float)p.Value.Damage / maxData;
            i++;
        }
    }
}
