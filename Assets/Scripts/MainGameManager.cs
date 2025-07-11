using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public GameObject[] sounds;
    private AudioSource music;
    public Button playBtn;
    public static MainGameManager instance;
    [HideInInspector] public bool isMobile = false;

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();
#endif
    private void CheckIfMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();
#endif
    }

    void Awake()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
        if (musicObjs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        CheckIfMobile();
        PlayerPrefs.DeleteAll();
        if (!isMobile)
        {
            PlayerPrefs.SetInt("GameMode", 1);
            playBtn.gameObject.GetComponent<SceneTransition>().scene = "Player Select";
        }
        else
        {
            playBtn.gameObject.GetComponent<SceneTransition>().scene = "Player Select";
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        changeMusicState();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        sounds = GameObject.FindGameObjectsWithTag("Sound");
        changeSoundState();
        //changeMusicState();
    }

    private void changeMusicState()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            music.enabled = true;
        }
        else
        {
            music.enabled = false;
        }
    }
    private void Update()
    {
        //sounds = GameObject.FindGameObjectsWithTag("Sound");
        //changeSoundState();
        changeMusicState();
    }
    private void changeSoundState()
    {
        if (PlayerPrefs.GetInt("sound") == 0)
        {
            foreach (GameObject Sound in sounds)
            {
                if (!Sound.GetComponent<AudioSource>().enabled)
                {
                    Sound.GetComponent<AudioSource>().enabled = true;
                }
            }
        }
        else
        {
            foreach (GameObject Sound in sounds)
            {
                if (Sound.GetComponent<AudioSource>().enabled)
                {
                    Sound.GetComponent<AudioSource>().enabled = false;
                }
            }
        }
    }
}
