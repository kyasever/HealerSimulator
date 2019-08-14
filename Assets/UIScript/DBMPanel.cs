using System.Collections;
using System.Collections.Generic;
using HealerSimulator;
using UnityEngine;
using UnityEngine.UI;

public class DBMPanel : MonoBehaviour
{
    public GameObject SkadaHUDPrefeb;

    public List<SkadaHUD> skadaHUDs = new List<SkadaHUD>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var skadaHUD = Instantiate(SkadaHUDPrefeb, transform).GetComponent<SkadaHUD>();
            skadaHUD.gameObject.SetActive(false);
            skadaHUDs.Add(skadaHUD);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMode.Instance.Boss == null)
            return;
        Refresh(GameMode.Instance.Boss);
    }

    public void Refresh(Character c)
    {
        var list =c.SkillList;
        if (list.Count == 0)
        {
            return;
        }

        int i = 0;

        foreach (var s in list)
        {
            skadaHUDs[i].gameObject.SetActive(true);
            skadaHUDs[i].NameLabel.text = s.skillName;
            skadaHUDs[i].AmountLabel.text = Utils.GetNString(s.CDRelease, s.CD);
            skadaHUDs[i].Slider.FillAmount = (float)s.CDRelease / s.CD;
            i++;
        }
    }
}
