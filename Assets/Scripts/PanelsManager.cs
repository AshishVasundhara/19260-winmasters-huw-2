using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    private ShopManager shop;
    private void Start()
    {
        shop = FindObjectOfType<ShopManager>();
    }
    public void ResetCounter()
    {
        shop.clothesCurrentItem = 0;
    }
}
