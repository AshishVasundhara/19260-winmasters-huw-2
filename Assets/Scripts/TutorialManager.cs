using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject mobileTuto;
    public GameObject pcTuto;
    private MainGameManager MGM;
    private void Start()
    {
        MGM = FindObjectOfType<MainGameManager>();
        if (PlayerPrefs.GetInt("GameMode") == 1)
        {
            if (!MGM.isMobile)
            {
                if (PlayerPrefs.HasKey("FTP"))
                {
                    pcTuto.SetActive(false);
                }
                else
                {
                    StartCoroutine(tutoTime(pcTuto));
                }
            }
            else
            {
                if (PlayerPrefs.HasKey("FTP"))
                {
                    mobileTuto.SetActive(false);
                }
                else
                {
                    StartCoroutine(tutoTime(mobileTuto));
                }
            }
        }

    }

    IEnumerator tutoTime(GameObject visuals)
    {
        yield return new WaitForSeconds(4f);
        visuals.SetActive(true);
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetString("FTP", "YES");
        visuals.SetActive(false);
    }

}
