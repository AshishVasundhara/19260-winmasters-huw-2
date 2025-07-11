using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopItemSO : ScriptableObject
{
    public float price;
    public Sprite image;
    //0 is LOCKED
    public int lockState = 0;
    
}
