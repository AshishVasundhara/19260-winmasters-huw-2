using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float resistanceForce = 10f;
    private bool isOnDohyo;
    private bool isThrusting= false;
    [HideInInspector]public bool isPushing= false;
    private bool canPush= false;
    private bool canThrust= false;
    [HideInInspector]public Dohyo dohyo;
    public float sumoMass = 1f;
    
    [Header("Pushing")]
    public float pushForce = 1.2f;
    public float maxPushTimer = 50f;
    private float currPushTimer;
    public float refillPushTimer = 50f;
    public Image pushSlider;
    [Header("Thrusting")]
    public float thrustForce = 70f;
    public float maxThrustTimer = 10f;
    public float singleTrhustTime = 5;
    private float currThrustTimer;
    public float refillThrustTimer = 50f;
    public Image thrustSlider;
    public GameObject hands;
    public AudioSource thrustSfx;
    public GameObject thrustParticles;
    public GameObject deathParticles;
    [HideInInspector] public bool isRotating;
    public float rotateSpeed = 350f;
    private GameManager gm;
    public AudioSource hahaSFX;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dohyo = FindObjectOfType<Dohyo>();
        gm = FindObjectOfType<GameManager>();
        rb.mass = sumoMass;
        currPushTimer = maxPushTimer;
        currThrustTimer = maxThrustTimer;
    }
    private void Update()
    {
 
        if (dohyo.playersOnDohyo.Contains(this.transform))
        {
            isOnDohyo = true;
        }
        else
        {
            isOnDohyo = false;
        }
        if (isRotating && gm.gameOn)
        {
            this.transform.RotateAround(transform.position, new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
        }
        isRotating = true;

    }
    public void Push(int inputValue)
    {
        if(inputValue == 1 && !isThrusting && canPush)
        {
            isRotating = false;
            isPushing = true;
            rb.AddForce(transform.up * pushForce * Time.deltaTime * 300, ForceMode2D.Force);
            manageTimePush(isPushing, pushSlider, ref currPushTimer,maxPushTimer);
            hands.SetActive(true);
        }
        else
        {
            isPushing = false;
            rb.velocity = rb.velocity.normalized * Mathf.Max(0f, rb.velocity.magnitude - resistanceForce * Time.deltaTime * sumoMass);
            manageTimePush(isPushing, pushSlider, ref currPushTimer,maxPushTimer);
            hands.SetActive(false);

        }
    }
    public void Thrust(int inputValue)
    {
        if (inputValue == 1 && !isPushing)
        {
            isThrusting = true;
            manageTimeThrust(isThrusting, thrustSlider, ref currThrustTimer, maxThrustTimer, singleTrhustTime);
            if (canThrust)
            {
                thrustSfx.Play();
                isRotating = false;
                Instantiate(thrustParticles.gameObject, this.transform.position, Quaternion.identity) ;
                rb.AddForce(transform.up * thrustForce * Time.deltaTime * 50, ForceMode2D.Impulse);
            }
        }
        else { 
            isThrusting = false;
            manageTimeThrust(isThrusting, thrustSlider, ref currThrustTimer, maxThrustTimer, singleTrhustTime);
        }
    }

    private void manageTimePush(bool isPushing, Image slide, ref float currentTimeCooling, float maxTimer)
    {
        if (isPushing && currentTimeCooling > 0)
        {
            currentTimeCooling -= Time.deltaTime;
        }
        //push force is too low and need time to refill
        else if (isPushing && currentTimeCooling <= maxTimer / 2)
        {
            canPush = false;
        }
        //filling force
        else if(!isPushing && currentTimeCooling <= maxTimer)
        {
            currentTimeCooling += Time.deltaTime * refillPushTimer;
            canPush = true;
        }
        slide.fillAmount = currentTimeCooling / maxTimer;
    }
    private void manageTimeThrust(bool isThrusting, Image slide, ref float currentTimeCooling, float maxThrustTimer,float thrustTime)
    {        
        //can again performe thrust attack
        if (currentTimeCooling > thrustTime)
        {
            canThrust = true;
        }
        //input pressed and current time is bigger than one unit of the time
        if (isThrusting && currentTimeCooling > thrustTime)
        {       
            //performe thrusting attack
            currentTimeCooling -= thrustTime;
        }
        else if (isThrusting && currentTimeCooling < thrustTime)
        {
            //cannot performe thrusting attack 
            //waiting to reload
            canThrust = false;
        }
        //fill bar => recharge when fill bar is not full
        if (!isThrusting && currentTimeCooling <= maxThrustTimer)
        {
            currentTimeCooling += Time.deltaTime * refillThrustTimer;
        }
        //update the silder fill ammount any way :)
        slide.fillAmount = currentTimeCooling / maxThrustTimer;
    }
}

