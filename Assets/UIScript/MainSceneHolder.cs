using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneHolder : MonoBehaviour
{
    public static MainSceneHolder Instance;

    public GameEndPanel GameEndPanel;

    private void Awake()
    {
        Instance = this;       
    }
}
