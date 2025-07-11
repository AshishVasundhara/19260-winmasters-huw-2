using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonsControls : MonoBehaviour
{
    private CharacterController cc;
    private bool pressing = false;
    void Start()
    {
        cc = GetComponent<CharacterController>();    
    }

    public void isPressingDown()
    {
        pressing = true;
    }
    public void isPressingUp()
    {
        pressing = false;
    }
    void Update()
    {
        if (!pressing)
        {
            cc.Push(0);
        }
        else
        {
            cc.Push(1);
        }
        cc.Thrust(0);
    }
    public void btnThrust()
    {
        cc.Thrust(1);
    }
}
