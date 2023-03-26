using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public void LaunchFirst()
    {
        GameConfigHandler.instance.LoadFirstLevel();
    }

    public void GotoNextLevel()
    {
        GameConfigHandler.instance.GotoNextLevel();
    }

    public void ReloadLevel()
    {
        GameConfigHandler.instance.RestartLevel();
    }

    public void ReturnToMenu()
    {
        GameConfigHandler.instance.ReturnToMenu();
    }
}
