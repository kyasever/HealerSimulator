using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillPanel : MonoBehaviour
{
    public GameObject SkillHUDPrefeb;

    public Character sourceCharacter;

    // Start is called before the first frame update
    void Start()
    {
        sourceCharacter = GameMode.Instance.Player;

        foreach(var v in sourceCharacter.SkillList)
        {
            var obj = Instantiate(SkillHUDPrefeb, transform);
            obj.GetComponent<SkillHUD>().sourceSkill = v;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
