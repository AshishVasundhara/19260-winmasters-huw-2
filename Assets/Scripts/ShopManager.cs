using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ShopManager : MonoBehaviour
{
    public int clothesCurrentItem = 1;
    public int foodCurrentItem = 1;
    private string activePanel = "HairCount";
    public Button shopClothesBtn;
    public Button shopFoodBtn;
    private float money = 200f;
    Transform activeShopPanel = null;
    Transform activeSumoPanel = null;
    public Transform foodPanel;
    public Text weightDisplayText;
    public Text thrustDisplayText;
    public Text pushDisplayText;
    public Text speedDisplayText;
    public Text moneyDisplayText;
    
    [Serializable]
    public struct shopHairs_sumoHair
    {
        public Transform shopHairs;
        public Transform sumoHairs;
    }
    [Serializable]
    public struct shopUnders_sumoUnders
    {
        public Transform shopUnders;
        public Transform sumoUnders;
    }
    [Serializable]
    public struct shopAcc_sumoAcc
    {
        public Transform shopAcc;
        public Transform sumoAcc;
    }
    [Serializable]
    public struct shopBodies_sumoBodies
    {
        public Transform shopBodies;
        public Transform sumoBodies;
    }
    [SerializeField]
    public shopHairs_sumoHair Hairs;
    [SerializeField]
    public shopUnders_sumoUnders underWares;
    [SerializeField]
    public shopAcc_sumoAcc acc;    
    [SerializeField]
    public shopBodies_sumoBodies bodies;
    private List<Transform> allPanels = new List<Transform>();
    public AudioSource shopBuySFX;
    private void Start()
    {

        LoadSavedData();

        //get player sumo data
        if (!PlayerPrefs.HasKey("Weight"))
        {
            PlayerPrefs.SetFloat("Weight", 320);
            PlayerPrefs.SetFloat("Push", 15);
            PlayerPrefs.SetFloat("Thrust", 17);
            PlayerPrefs.SetFloat("Speed", 20);
        }
        else
        {
            weightDisplayText.text = PlayerPrefs.GetFloat("Weight").ToString() + " POUNDS";
            pushDisplayText.text = PlayerPrefs.GetFloat("Push").ToString() + " %";
            thrustDisplayText.text = PlayerPrefs.GetFloat("Thrust").ToString() + " %";
            speedDisplayText.text = PlayerPrefs.GetFloat("Speed").ToString() + " %";
        }

        //initialize active section 
        activeShopPanel = Hairs.shopHairs;
        activeSumoPanel = Hairs.sumoHairs;
        activePanel = "HairCount";

        //first time play game
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetFloat("money", money);
        }
        money = PlayerPrefs.GetFloat("money");
        moneyDisplayText.text = PlayerPrefs.GetFloat("money").ToString() + "$";
    }
    public void NextPrevButton(bool isNext)
    {
        if (clothesCurrentItem > activeShopPanel.childCount - 2 && isNext)
        {
            clothesCurrentItem = 0;
        }
        else if (clothesCurrentItem == 0 && !isNext)
        {
            clothesCurrentItem = activeShopPanel.childCount - 1;
        }
        else
        {
            if (isNext)
            {
                clothesCurrentItem++;
            }
            else
            {
                clothesCurrentItem--;
            }
        }
        activeShopPanel.GetChild(clothesCurrentItem).gameObject.SetActive(true);
        int lockState = activeShopPanel.GetChild(clothesCurrentItem).GetComponent<SingleShopItem>().shopItemSO.lockState;
        lockState = PlayerPrefs.GetInt(activeShopPanel.GetChild(clothesCurrentItem).gameObject.name,
            activeShopPanel.GetChild(clothesCurrentItem).GetComponent<SingleShopItem>().shopItemSO.lockState);
        if (lockState == 0)
        {
            shopClothesBtn.GetComponentInChildren<Text>().text = "BUY " + activeShopPanel.GetChild(clothesCurrentItem).gameObject.GetComponent<SingleShopItem>().shopItemSO.price.ToString();
            shopClothesBtn.onClick.AddListener(delegate
            {
                buyItem(activeShopPanel.GetChild(clothesCurrentItem).gameObject.GetComponent<SingleShopItem>().shopItemSO.price);
            });
        }
        else if (lockState == 1)
        {
            shopClothesBtn.GetComponentInChildren<Text>().text = "SELECT";
            shopClothesBtn.onClick.AddListener(delegate
            {
                PlayerPrefs.SetInt(activePanel, clothesCurrentItem);
            });
        }
        activeSumoPanel.GetChild(clothesCurrentItem).gameObject.SetActive(true);

        for (int i = 0; i < activeShopPanel.transform.childCount; i++)
        {
            if (i != clothesCurrentItem)
            {
                activeShopPanel.GetChild(i).gameObject.SetActive(false);
                activeSumoPanel.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void LoadSavedData()
    {
        for (int i = 0; i < Hairs.shopHairs.childCount; i++)
        {
            allPanels.Add(Hairs.shopHairs.GetChild(i).transform);
        }
        for (int i = 0; i < Hairs.sumoHairs.childCount; i++)
        {
            allPanels.Add(Hairs.sumoHairs.GetChild(i).transform);
        }

        for (int i = 0; i < underWares.shopUnders.childCount; i++)
        {
            allPanels.Add(underWares.shopUnders.GetChild(i).transform);
        }
        for (int i = 0; i < underWares.sumoUnders.childCount; i++)
        {
            allPanels.Add(underWares.sumoUnders.GetChild(i).transform);
        }

        for (int i = 0; i < acc.shopAcc.childCount; i++)
        {
            allPanels.Add(acc.shopAcc.GetChild(i).transform);
        }
        for (int i = 0; i < acc.sumoAcc.childCount; i++)
        {
            allPanels.Add(acc.sumoAcc.GetChild(i).transform);
        }

        for (int i = 0; i < bodies.shopBodies.childCount; i++)
        {
            allPanels.Add(bodies.shopBodies.GetChild(i).transform);
        }
        for (int i = 0; i < bodies.sumoBodies.childCount; i++)
        {
            allPanels.Add(bodies.sumoBodies.GetChild(i).transform);
        }
        foreach (Transform panel in allPanels)
        {
            panel.gameObject.SetActive(false);
        }
        Hairs.sumoHairs.GetChild(PlayerPrefs.GetInt("HairCount")).gameObject.SetActive(true);
        Hairs.shopHairs.GetChild(PlayerPrefs.GetInt("HairCount")).gameObject.SetActive(true);
        underWares.sumoUnders.GetChild(PlayerPrefs.GetInt("UnderCount")).gameObject.SetActive(true);
        underWares.shopUnders.GetChild(PlayerPrefs.GetInt("UnderCount")).gameObject.SetActive(true);
        acc.sumoAcc.GetChild(PlayerPrefs.GetInt("AccCount")).gameObject.SetActive(true);
        acc.shopAcc.GetChild(PlayerPrefs.GetInt("AccCount")).gameObject.SetActive(true);
        bodies.sumoBodies.GetChild(PlayerPrefs.GetInt("BodyCount")).gameObject.SetActive(true);
        bodies.shopBodies.GetChild(PlayerPrefs.GetInt("BodyCount")).gameObject.SetActive(true);
        clothesCurrentItem = PlayerPrefs.GetInt(activePanel);

    }
    public void buyItem(float price)
    {
        if(price <= money && activeShopPanel.GetChild(clothesCurrentItem).GetComponent<SingleShopItem>().shopItemSO.lockState != 1)
        {
            money -= price;
            updateMoney();
            shopBuySFX.Play();
            moneyDisplayText.text = PlayerPrefs.GetFloat("money").ToString() + "$";
            shopClothesBtn.GetComponentInChildren<Text>().text = "SELECT";
            activeShopPanel.GetChild(clothesCurrentItem).GetComponent<SingleShopItem>().shopItemSO.lockState = 1;
            PlayerPrefs.SetInt(activeShopPanel.GetChild(clothesCurrentItem).gameObject.name, activeShopPanel.GetChild(clothesCurrentItem).GetComponent<SingleShopItem>().shopItemSO.lockState);
            PlayerPrefs.SetInt(activePanel, clothesCurrentItem);
        }
        else
        {
            return;
        }
    }
    public void updateCounter()
    {
        if (Hairs.shopHairs.gameObject.activeInHierarchy)
        {
            activeShopPanel = Hairs.shopHairs;
            activeSumoPanel = Hairs.sumoHairs;
            activePanel = "HairCount";
        }
        else if (underWares.shopUnders.gameObject.activeInHierarchy)
        {
            activeShopPanel = underWares.shopUnders;
            activeSumoPanel = underWares.sumoUnders;
            activePanel = "UnderCount";
        }
        else if (bodies.shopBodies.gameObject.activeInHierarchy)
        {
            activeShopPanel = bodies.shopBodies;
            activeSumoPanel = bodies.sumoBodies;
            activePanel = "BodyCount";

        }
        else if (acc.shopAcc.gameObject.activeInHierarchy)
        {
            activeShopPanel = acc.shopAcc;
            activeSumoPanel = acc.sumoAcc;
            activePanel = "AccCount";
        }
        clothesCurrentItem = PlayerPrefs.GetInt(activePanel);
        shopClothesBtn.GetComponentInChildren<Text>().text = "SELECT";
    }
    public void FoodShopButtons(bool isNext){
        if (foodCurrentItem > foodPanel.childCount - 2 && isNext)
        {
            foodCurrentItem = 0;
        }
        else if (foodCurrentItem == 0 && !isNext)
        {
            foodCurrentItem = foodPanel.childCount - 1;
        }
        else
        {
            if (isNext)
            {
                foodCurrentItem++;
            }
            else
            {
                foodCurrentItem--;
            }
        }
        foodPanel.GetChild(foodCurrentItem).gameObject.SetActive(true);
        for (int j = 0; j < foodPanel.transform.childCount; j++)
        {
            if (j != foodCurrentItem)
            {
                foodPanel.GetChild(j).gameObject.SetActive(false);
            }
        }
        UpdateFoodShopButton();
    }
    public void resetCounter()
    {
        foodCurrentItem = 0;
        foreach (Transform meal in foodPanel)
        {
            meal.gameObject.SetActive(false);
        }
        foodPanel.GetChild(0).gameObject.SetActive(true);
        UpdateFoodShopButton();
        LoadSavedData(); 
    }
    private void UpdateFoodShopButton()
    {
        SingleFoodItem afood = foodPanel.GetChild(foodCurrentItem).GetComponent<SingleFoodItem>();
        shopFoodBtn.GetComponentInChildren<Text>().text = "BUY " + afood.foodso.price.ToString();
        shopFoodBtn.onClick.RemoveAllListeners(); // remove previous listeners
        shopFoodBtn.onClick.AddListener(delegate
        {
            BuyFood(afood);
        });
    }
    private void BuyFood(SingleFoodItem afood)
    {
        if(money >= afood.foodso.price)
        {
            money -= afood.foodso.price;
            updateMoney();

            float w = PlayerPrefs.GetFloat("Weight") + afood.foodso.addedWeight;
            weightDisplayText.text = w.ToString() + " POUNDS";
            PlayerPrefs.SetFloat("Weight", w);
            float p = PlayerPrefs.GetFloat("Push") + afood.foodso.addedPushingForce;
            pushDisplayText.text = p.ToString() + " %";
            PlayerPrefs.SetFloat("Push", p);
            float t = PlayerPrefs.GetFloat("Thrust") + afood.foodso.addedThrustingForce;
            thrustDisplayText.text = t.ToString() + " %";
            PlayerPrefs.SetFloat("Thrust", t);
            float s = PlayerPrefs.GetFloat("Speed") + afood.foodso.addedSpeed;
            speedDisplayText.text = s.ToString() + " %";
            PlayerPrefs.SetFloat("Speed", s);
        }
       
    }
    private void updateMoney()
    {
        PlayerPrefs.SetFloat("money", money);
        moneyDisplayText.text = PlayerPrefs.GetFloat("money").ToString() + "$";
    }
}
