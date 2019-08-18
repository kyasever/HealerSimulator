using HealerSimulator;
using System.Collections.Generic;
using UnityEngine;

//这个比较特殊,它是每秒钟更新一次.所以不绑定任何数据
public class SkadaPanel : MonoBehaviour
{
    private ObjectPool pool;

    public float updateTime = 1f;

    private void Start()
    {
        pool = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    private void Update()
    {
        //每秒钟更新一次
        updateTime -= Time.deltaTime;
        if (updateTime < 0)
        {
            updateTime = 1f;
            Refresh();
        }

    }

    public void Refresh()
    {
        List<KeyValuePair<Character, SkadaData>> data = Skada.Instance.GetDamageRank();
        if (data.Count == 0)
        {
            return;
        }

        int count = data.Count;
        List<GameObject> list = pool.GetInstantiate(count);

        int maxData = data[0].Value.Damage;
        if (maxData == 0)
        {
            maxData = 1;
        }

        for (int i = 0; i < data.Count; i++)
        {
            SkadaHUD com = list[i].GetComponent<SkadaHUD>();
            com.gameObject.SetActive(true);
            com.NameLabel.text = data[i].Key.CharacterName;
            com.AmountLabel.text = (data[i].Value.Damage).ToString();
            com.Slider.FillAmount = (float)data[i].Value.Damage / maxData;
        }
    }
}
