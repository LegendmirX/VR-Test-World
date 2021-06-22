using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBootUp : MonoBehaviour 
{
    public enum BootMode
    {
        NewGame,
        LoadGame
    }
    public static BootMode bootMode = BootMode.NewGame;

    List<Action> bootUpList;

    private void Awake()
    {
        bootUpList = new List<Action>();

        switch (bootMode)
        {
            case BootMode.NewGame:
                NewGame();
                break;
            case BootMode.LoadGame:
                NewGame(); //TODO: Impliment Load Game
                break;
        }


        for (int i = 0; i < bootUpList.Count; i++)
        {
            bootUpList[i].Invoke();
        }
        
    }

    void NewGame()
    {
        bootUpList.Add(FindObjectOfType<GameAssets>().SetUp);
        bootUpList.Add(SoundManager.SetUp);
    }
}
