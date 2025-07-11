using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public void selectGameMode(int gameModeIndex)
    {
        PlayerPrefs.SetInt("GameMode", gameModeIndex);
    }
}
