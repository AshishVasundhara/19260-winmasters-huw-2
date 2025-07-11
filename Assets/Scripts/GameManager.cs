using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public enum gamemodes
    {
        solo,
        duo,
        tri,
        quattro
    }
    public gamemodes gameModes;

    [HideInInspector] public bool gameOn = false;
    private Dohyo dohyo;
    [HideInInspector] public bool roundIsEnd = false;
    private int currRoundCount = 0;
    [HideInInspector] public bool winGame = false;
    [HideInInspector] public bool loseGame = false;
    [HideInInspector] public bool gameOver;
    public GameObject[] startingPositions;
    [Serializable]
    public struct aPlayer
    {
        public GameObject player;
        public int winsCount;
        public int loseCount;
    }
    public aPlayer[] Players;
    public int maxRounds;
    private int winsCount;
    public EventHandler<onWinPlayer> onWinCountChange;
    public class onWinPlayer : EventArgs
    {
        public GameObject eventPlayer;
    }

    public event EventHandler onWinGame;
    private UImanager ui;
    public float totalTime = 60f;
    private float timeLeft;
    public Text countdownText;
    [HideInInspector] public int award;
    public Transform[] adversairesPrefabs;
    public Transform[] localPlayersPrefabs;

    public Transform soloPlayerPrefab;
    private int currAdv;

    public GameObject multiPlayerWinPanel;
    public Text winPlayerText;
    void Start()
    {
        getGameMode();

        //manage dhoyo
        dohyo = FindObjectOfType<Dohyo>();
        dohyo.gameObject.GetComponent<SpriteRenderer>().sprite = dohyo.dhoyos[PlayerPrefs.GetInt("SelectedMap")];



        onWinCountChange += GameManager_onWinCountChange;
        onWinGame += GameManager_onWinGame;

        //ui
        ui = FindObjectOfType<UImanager>();
        timeLeft = totalTime;
        setUpPlayer();


    }

    private void GameManager_onWinGame(object sender, EventArgs e)
    {
        CalculeAward();
    }


    private void GameManager_onWinCountChange(object sender, onWinPlayer e)
    {
        winsCount++;
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i].player == e.eventPlayer.gameObject)
            {
                Players[i].winsCount++;
            }
        }
    }

    void Update()
    {
        if (!roundIsEnd)
        {
            checkIfRoundIsEnd();
        }
        else
        {
            if (!gameOver)
            {
                StartCoroutine(startNewRoundDelayed());
            }
        }



        if (currRoundCount == maxRounds) // game over !
        {
            gameOver = true;
            setClassement();
            gameOn = false;


            //check result
            if (winGame)
            {
                //gameover and won
                PlayerPrefs.SetString((currAdv + 1).ToString() + "adv", "unlocked");
                PlayerPrefs.SetInt("CurrAdv", currAdv + 1);
                onWinGame?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                award = 0;
            }

        }
        else
        {
            gameOver = false;
        }

        if (gameOn)
        {
            countDownTimer();
        }
        if (timeLeft <= 0f)
        {
            gameOver = true;
            roundIsEnd = true;
        }
    }
    IEnumerator startNewRoundDelayed()
    {
        yield return new WaitForSeconds(1f);
        startNewRound();
    }
    private void CalculeAward()
    {
        award = Mathf.RoundToInt((timeLeft * 0.8f) * winsCount);
        PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money") + award);
        onWinGame -= GameManager_onWinGame;
    }
    private void startNewRound()
    {
        setUpPlayer();
        loseGame = false;
        winGame = false;
        roundIsEnd = false;
    }
    private void checkIfRoundIsEnd()
    {
        if (dohyo.playersOnDohyo.Count <= 1)//one still alive on dhoyo
        {
            roundIsEnd = true;
            if (!gameOver)
            {
                //won round
                onWinCountChange?.Invoke(this, new onWinPlayer
                {
                    eventPlayer = dohyo.playersOnDohyo[0].gameObject
                });

            }

            //set loosers for this round
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].player != dohyo.playersOnDohyo[0].gameObject)
                {
                    Players[i].loseCount++;
                }
            }


            //work on ui for final result
            if (gameModes == gamemodes.solo)
            {
                if (dohyo.playersOnDohyo[0].GetComponent<DesktopControls>() != null)
                {
                    ui.roundsResults[currRoundCount].sprite = ui.tick;
                }
                else
                {
                    ui.roundsResults[currRoundCount].sprite = ui.cross;
                }
            }

            //get curr round number
            currRoundCount = Players[0].winsCount + Players[0].loseCount;
        }
    }
    private void setClassement()
    {
        if (gameModes == gamemodes.solo)
        {
            //win at least 2 times
            if (Players[0].winsCount >= maxRounds - 1 && gameOver)
            {
                winGame = true;
                loseGame = false;
            }
            else
            {
                winGame = false;
                loseGame = true;
            }
        }
        else
        {
            int maxWins = Players[0].winsCount;
            for (int i = 1; i < Players.Length; i++)
            {
                if (Players[i].winsCount > maxWins)
                {
                    maxWins = Players[i].winsCount;
                    GameObject winnerWinnerChikenDinner = Players[i].player;
                    //print on ui the winner
                    //text under the player photo
                    winPlayerText.text = winnerWinnerChikenDinner.name;
                    multiPlayerWinPanel.SetActive(true);
                }
            }
        }

    }
    private void setUpPlayer()
    {
        dohyo.playersOnDohyo.Clear();
        int posIndex = 0;

        foreach (var p in Players)
        {
            dohyo.playersOnDohyo.Add(p.player.transform);
            p.player.SetActive(true);
            if(p.player.GetComponentInChildren<Canvas>() != null)
            {
                p.player.GetComponentInChildren<Canvas>().transform.SetParent(null);
            }
            p.player.transform.position = startingPositions[posIndex].transform.position;
            posIndex++;
        }
    }
    private void countDownTimer()
    {
        timeLeft -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);

        countdownText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    private void getGameMode()
    {
        int gameModeIndex = PlayerPrefs.GetInt("GameMode");
        if (gameModeIndex == 1)
        {
            //is solo
            gameModes = gamemodes.solo;
            Players = new aPlayer[2];

            //get curr adv 
            currAdv = PlayerPrefs.GetInt("CurrAdv");


            GameObject aiClone = Instantiate(adversairesPrefabs[currAdv].gameObject);
            Players[1].player = aiClone;

            GameObject playerClone = Instantiate(soloPlayerPrefab.gameObject);
            Players[0].player = playerClone;


        }
        else if (gameModeIndex == 2)
        {
            Players = new aPlayer[2];

            //is duo
            gameModes = gamemodes.duo;
            for (int i = 0; i <= 1; i++)
            {
                GameObject playerClone = Instantiate(localPlayersPrefabs[i].gameObject);
                Players[i].player = playerClone;
            }

        }
        else if (gameModeIndex == 3)
        {
            Players = new aPlayer[3];

            //is trio
            gameModes = gamemodes.tri;
            for (int i = 0; i <= 2; i++)
            {
                GameObject playerClone = Instantiate(localPlayersPrefabs[i].gameObject);
                Players[i].player = playerClone;
            }
        }
        else if (gameModeIndex == 4)
        {
            Players = new aPlayer[4];
            
            //is quatro
            gameModes = gamemodes.quattro;
            for (int i = 0; i <= 3; i++)
            {
                GameObject playerClone = Instantiate(localPlayersPrefabs[i].gameObject);
                Players[i].player = playerClone;
            }
        }
    }
}
