using HealerSimulator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public int ControllerNum { get => GameMode.Instance.UpdateEvent.GetInvocationList().Length; }

    public int TeamCharacterCount { get => GameMode.Instance.TeamCharacters.Count; }

    public int DeathCharacterCount { get => GameMode.Instance.DeadCharacters.Count; }


    // Start is called before the first frame update
    void Awake()
    {
        GameMode.Instance.InitGame(5);
    }

    // Update is called once per frame
    void Update()
    {
        GameMode.Instance.UpdateEvent?.Invoke();
    }
}
