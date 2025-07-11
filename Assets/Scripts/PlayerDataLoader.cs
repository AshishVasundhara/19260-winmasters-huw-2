using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite[] hairs;
    public Sprite[] accs;
    public Sprite[] underwares;
    public Sprite[] bodies;
    public Sprite[] hands;
    [Header("Player Parts")]
    public SpriteRenderer hairTopDown;
    public SpriteRenderer bodyTopDown;
    public SpriteRenderer accTopDown;
    public SpriteRenderer handAccTopDown;
    public SpriteRenderer underwareTopDown;
    public SpriteRenderer handsTopDown;

    [Header("Player Canvas Preview")]
    public Transform canvasHairParent;
    public Transform canvasBodyParent;
    public Transform canvasAccParent;
    public Transform canvasUnderwareParent;
    private CharacterController CC;
    private DesktopControls player;
    void Start()
    {
        CC = GetComponent<CharacterController>();
        player = GetComponent<DesktopControls>();


        CC.pushForce += (CC.pushForce * (PlayerPrefs.GetFloat("Push")/2))/100;
        CC.refillPushTimer += (CC.pushForce * (PlayerPrefs.GetFloat("Push")/2)) / 100;

        CC.thrustForce += (CC.thrustForce * (PlayerPrefs.GetFloat("Thrust")/2)) / 100;
        CC.refillThrustTimer += (CC.thrustForce * (PlayerPrefs.GetFloat("Thrust")/5)) / 100;
        

        //300 wright is eqv to 10 game weight
        CC.sumoMass += (CC.sumoMass * (PlayerPrefs.GetFloat("Weight"))/30) / 100;

        CC.rotateSpeed += CC.rotateSpeed * (PlayerPrefs.GetFloat("Speed")/3) / 100;


        //update the canvas 
        canvasHairParent.GetChild(PlayerPrefs.GetInt("HairCount")).gameObject.SetActive(true);
        canvasUnderwareParent.GetChild(PlayerPrefs.GetInt("UnderCount")).gameObject.SetActive(true);
        canvasAccParent.GetChild(PlayerPrefs.GetInt("AccCount")).gameObject.SetActive(true);
        canvasBodyParent.GetChild(PlayerPrefs.GetInt("BodyCount")).gameObject.SetActive(true);

        hairTopDown.sprite = hairs[PlayerPrefs.GetInt("HairCount")];

        underwareTopDown.sprite = underwares[PlayerPrefs.GetInt("UnderCount")];
        bodyTopDown.sprite = bodies[PlayerPrefs.GetInt("BodyCount")];
        handsTopDown.sprite = hands[PlayerPrefs.GetInt("BodyCount")];
        
        if (PlayerPrefs.GetInt("AccCount") == 2 || PlayerPrefs.GetInt("AccCount") == 1)
        {
            accTopDown.gameObject.SetActive(false);
            handAccTopDown.gameObject.SetActive(true);
            handAccTopDown.sprite = accs[PlayerPrefs.GetInt("AccCount")];
        }
        else
        {
            accTopDown.gameObject.SetActive(true);
            handAccTopDown.gameObject.SetActive(false);
            accTopDown.sprite = accs[PlayerPrefs.GetInt("AccCount")];
        }
    }
}
