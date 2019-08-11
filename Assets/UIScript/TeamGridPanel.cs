using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HealerSimulator;
using UnityEngine.UI;
using System.Text;

public class TeamGridPanel : MonoBehaviour
{
    public CharacterHUD CharacterHUDPrefeb;

    public List<CharacterHUD> hpBoxList = new List<CharacterHUD>();

    private GameMode game = GameMode.Instance;

    void Start()
    {
        foreach (var c in game.TeamCharacters)
        {
            var hud = CreateCharacterHUD();
            hud.sourceCharacter = c;
            hpBoxList.Add(hud);
        }
    }

    public CharacterHUD CreateCharacterHUD()
    {
        var obj = Instantiate(CharacterHUDPrefeb, transform);
        return obj.GetComponent<CharacterHUD>();
    }

}