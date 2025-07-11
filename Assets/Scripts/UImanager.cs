using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    private GameManager GM;
    public GameObject resultPanel;
    public GameObject infoPanel;
    public Text resultTextDisplay;
    public Text awardTextDisplay;
    public Image[] roundsResults;
    public Sprite tick;
    public Sprite cross;
    public Transform countDownPanel;
    public Text countdownText;
    public AudioSource fightSFX;
    public AudioSource bipSFX;
   
    public Sprite winBg;
    public Sprite winRuban;
    public Sprite loseBg;
    public Sprite loseRuban;
    public Image ruban;
    public Image bg;

    private void Start()
    {
        GM = FindObjectOfType<GameManager>();
        resultPanel.SetActive(false);
        countDownPanel.gameObject.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }
    private void Update()
    {
        if (GM.roundIsEnd && GM.gameModes == GameManager.gamemodes.solo)
        {
            if (GM.gameOver)
            {
                resultPanel.SetActive(true);

                if (GM.winGame)
                {
                    resultTextDisplay.text = "YOU WIN !";
                    if (PlayerPrefs.GetInt("CurrAdv") == 3)
                    {
                        infoPanel.SetActive(true);
                    }
                    ruban.sprite = winRuban;
                    bg.sprite = winBg;
                }
                else
                {
                    resultTextDisplay.text = "YOU LOST !";
                    ruban.sprite = loseRuban;
                    bg.sprite = loseBg;
                }
            }
        }
        if (!GM.gameOn)
        {
            awardTextDisplay.text = "+ " + GM.award.ToString() + " $";
        }
    }
    IEnumerator CountdownCoroutine()
    {
        bipSFX.Play();
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        
        bipSFX.Play();
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        
        bipSFX.Play();
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        
        fightSFX.Play();
        countdownText.text = "FIGHT !!!";
        yield return new WaitForSeconds(1f);
        
        countDownPanel.gameObject.SetActive(false);
        GM.gameOn = true;
    }
}
