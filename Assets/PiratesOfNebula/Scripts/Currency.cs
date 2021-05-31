using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    public int Money;
    private int money;
    public TMP_Text MoneyUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        if (Money != money)
        {
            money = Money;
            MoneyUI.text = $"Units: {Money}";
        }
    }
    
}
