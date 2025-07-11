using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{

    public GameObject musicOnButton;
    public GameObject musicOffButton;
    public GameObject soundOnButton;
    public GameObject soundOffButton;


    private void Start()
    {
        changeMusicState();
        changeSoundState();
    }
    private void Update()
    {
        changeSoundState();
    }

    private void changeMusicState()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
        }
        else
        {
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
        }
    }

    private void changeSoundState()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {

            soundOnButton.SetActive(true);
            soundOffButton.SetActive(false);
        }
        else
        {
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
        }
    }
    public void onClickToggleMusic()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }
        changeMusicState();
    }
    public void onClickToggleSound()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            PlayerPrefs.SetInt("sound", 1);
            soundOnButton.SetActive(true);
            soundOffButton.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
        }
        changeSoundState();
    }
}