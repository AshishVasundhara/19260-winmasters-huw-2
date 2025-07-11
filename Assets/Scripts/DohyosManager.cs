using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class DohyosManager : MonoBehaviour
{
    [Serializable]
    public struct maps_Data
    {
        public Sprite mapImage;
        public bool isLocked;
    }
    private int mapIndex = 0;
    public GameObject lockImage;
    public Image mapImageHolder;
    public maps_Data[] maps;
    public Button selectButton;
    private void Start()
    {
        //player 2 is unlocked
        if (PlayerPrefs.HasKey("3adv"))
        {
            maps[1].isLocked = false;
        }
    }
    public void nextAdv()
    {
        mapIndex++;
        if (mapIndex == maps.Length)
        {
            mapIndex = 0;
        }
        mapImageHolder.sprite = maps[mapIndex].mapImage;

        LockUnlockMap(maps[mapIndex].isLocked);

    }
    public void prevAdv()
    {
        mapIndex--;
        if (mapIndex == -1)
        {
            mapIndex = maps.Length - 1;
        }
        mapImageHolder.sprite = maps[mapIndex].mapImage;
        LockUnlockMap(maps[mapIndex].isLocked);
    }
    public void LockUnlockMap(bool isLocked)
    {
        if (isLocked)
        {
            lockImage.SetActive(true);
            selectButton.interactable = false;
        }
        else
        {
            lockImage.SetActive(false);
            selectButton.interactable = true;

        }
    }
    public void selectMap()
    {
        PlayerPrefs.SetInt("SelectedMap", mapIndex);
    }
}
