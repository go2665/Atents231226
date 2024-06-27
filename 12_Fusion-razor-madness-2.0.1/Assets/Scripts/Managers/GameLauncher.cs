using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameLauncher : MonoBehaviour
{
    public GameObject LauncherPrefab;

    public void Launch(GameMode _gameMode, string _room)
    {
        FusionLauncher launcher = FindObjectOfType<FusionLauncher>();
        if (launcher == null)
            launcher = Instantiate(LauncherPrefab).GetComponent<FusionLauncher>();

        LevelManager lm = FindObjectOfType<LevelManager>();
        lm.Launcher = launcher;

        launcher.Launch(_gameMode, _room, lm);
    }
}
