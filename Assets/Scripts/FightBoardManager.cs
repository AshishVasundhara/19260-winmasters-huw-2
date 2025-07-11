using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class FightBoardManager : MonoBehaviour
{
    public InputField nameInput;
    public Text playerNameDisplay;
    public Text advNameDisplay;
    public GameObject lockImage;
    private int advIndex = 0;
    public Transform lockBackground;
    
    public Transform hairs;
    public Transform bodies;
    public Transform underwares;
    public Transform accs;

    public Button startBtn;
    [Serializable]
    public struct adv_Data
    {
        public string name;
        public GameObject preview;
        public bool isUnLocked;
        public GameObject statsPanel;
    }
    public adv_Data[] adversaires;

    void Start()
    {
        PlayerPrefs.SetString("0adv", "unlocked");
        checkLockedUnlockedAdv();

        hairs.GetChild(PlayerPrefs.GetInt("HairCount")).gameObject.SetActive(true);
        underwares.GetChild(PlayerPrefs.GetInt("UnderCount")).gameObject.SetActive(true);
        accs.GetChild(PlayerPrefs.GetInt("AccCount")).gameObject.SetActive(true);
        bodies.GetChild(PlayerPrefs.GetInt("BodyCount")).gameObject.SetActive(true);


        foreach (adv_Data adv in adversaires)
        {
            adv.preview.SetActive(false);
        }

        if (!PlayerPrefs.HasKey("CurrAdv"))
        {
            adversaires[0].preview.gameObject.SetActive(true);
        }
        else
        {
            adversaires[PlayerPrefs.GetInt("CurrAdv")].preview.gameObject.SetActive(true);
            advIndex = PlayerPrefs.GetInt("CurrAdv");
            Debug.Log(PlayerPrefs.GetInt("CurrAdv"));
        }

        if (PlayerPrefs.HasKey("name"))
        {
            playerNameDisplay.text = PlayerPrefs.GetString("name");
        }
        else
        {
            return;
        }
        PlayerPrefs.SetString("0adv", "unlocked");
        checkLockedUnlockedAdv();

 
    }
    public void saveNewName()
    {
        if(nameInput.text != "")
        {
            PlayerPrefs.SetString("name", nameInput.text.ToString());
            playerNameDisplay.text = PlayerPrefs.GetString("name");
        }
    }

    public void ActiveStatsPanel(bool active)
    {
        adversaires[advIndex].statsPanel.SetActive(active);
        advNameDisplay.gameObject.transform.parent.gameObject.SetActive(!active);
    }
    public void nextAdv()
    {
        advIndex++;
        foreach (adv_Data singleadv  in adversaires)
        {
            singleadv.preview.SetActive(false);
        }
        if(advIndex == adversaires.Length)
        {
            advIndex = 0;
        }
        adversaires[advIndex].preview.SetActive(true);
        advNameDisplay.text = adversaires[advIndex].name;
        lockUnlock(adversaires[advIndex].isUnLocked);
        adversaires[advIndex].statsPanel.SetActive(false);
        advNameDisplay.gameObject.transform.parent.gameObject.SetActive(true);

    }
    public void prevAdv()
    {
        advIndex--;
        foreach (adv_Data singleadv in adversaires)
        {
            singleadv.preview.SetActive(false);
        }
        if (advIndex == -1)
        {
            advIndex = adversaires.Length-1;
        }
        adversaires[advIndex].preview.SetActive(true);
        advNameDisplay.text = adversaires[advIndex].name;
        lockUnlock(adversaires[advIndex].isUnLocked);
        adversaires[advIndex].statsPanel.SetActive(false);
        advNameDisplay.gameObject.transform.parent.gameObject.SetActive(true);

    }
    private void checkLockedUnlockedAdv()
    {
        for (int i = 0; i < adversaires.Length; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString() + "adv"))
            {
                adversaires[i].isUnLocked = true ;
            }
        }
    }
    private void lockUnlock(bool isLocked)
    {
        if (!isLocked)
        {
            lockImage.SetActive(true);
            lockBackground.SetAsLastSibling();
            startBtn.interactable = false;
            
        }
        else
        {
            lockImage.SetActive(false);
            lockBackground.SetAsFirstSibling();
            startBtn.interactable = true;
            
        }
    }

}
